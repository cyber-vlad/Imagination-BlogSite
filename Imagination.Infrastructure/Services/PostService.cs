using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using Imagination.Infrastructure.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepositor;
        private readonly ICommentRepository _commentRepositor;

        public PostService(IPostRepository postRepository, IUserRepository userRepository, ILikeRepository likeRepositor, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _likeRepositor = likeRepositor;
            _commentRepositor = commentRepository;
        }

        public async Task<BaseResponse> CreatePostAsync(CreatePostDto model)
        {
            var post = new Post
            {
                Content = model.Content,
                Title = model.Title,
                Category = model.Category,
                DateOfCreation = DateTime.Now,
                NrLikes = 0,
                NrComments = 0,
                AuthorId = model.AuthorId,
            };
            var response = await _postRepository.AddPostAsync(post);
            if (response.ErrorCode == Domain.Enum.ErrorCode.NoError) return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.NoError };
            return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.Create_post_failed, ErrorMessage = "Create post failed"};
        }

        public async Task<List<PostDto>> GetAllPostsForCurrentUserAsync(int currentUserId)
        {
            var posts = await _postRepository.GetAllPostsAsync();

            var postDtos = new List<PostDto>();

            foreach (var post in posts)
            {
                var author = await _userRepository.GetUserByIdAsync(post.AuthorId);

                postDtos.Add(new PostDto
                {
                    PostId = post.Id,
                    Content = post.Content,
                    Title = post.Title,
                    Category = post.Category,
                    DateOfCreation = post.DateOfCreation,
                    NrLikes = post.NrLikes,
                    NrComments = post.NrComments,
                    Author = new AuthorPostDto
                    {
                        Username = author.Username,
                        PhotoUrl = author.PhotoUrl
                    },
                    IsLikedByCurrentUser = post.Likes?.Any(l => l.UserId == currentUserId) ?? false
                });
            }

            return postDtos;
        }

        public async Task<PostDto> GetPostDetailsByIdAsync(int postId)
        {
            var result = await _postRepository.GetPostByIdAsync(postId);
            if (result is null) return null;

            var authorPost = await _userRepository.GetUserByIdAsync(result.AuthorId);
            var post = new PostDto
            {
                PostId = result.Id,
                Content = result.Content,
                Title = result.Title,
                Category = result.Category,
                DateOfCreation = result.DateOfCreation,
                NrLikes = result.NrLikes,
                NrComments = result.NrComments,
                Author = new AuthorPostDto
                {
                    Username = authorPost.Username,
                    PhotoUrl= authorPost.PhotoUrl,
                },
            };

            return post;
        }

        public async Task<ToggledLikeResponse> ToggleLikeAsync(ToggleLikeDto model)
        {
            var response = await _likeRepositor.AddLikeAsync(model);
            if(response.ErrorCode == Domain.Enum.ErrorCode.Internal_error)
            {
                return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Like attribution failed"};
            }
            return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, Post = response.Post, IsLiked = response.IsLiked };
        }

        public async Task<CreatedCommentResponse> CreateCommentAsync(CreateCommentDto model)
        {
            var response = await _commentRepositor.AddCommentAsync(model);
            return new CreatedCommentResponse();
        }

    }
}
