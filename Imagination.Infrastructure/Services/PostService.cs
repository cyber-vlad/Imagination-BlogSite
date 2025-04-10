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

        public PostService(IPostRepository postRepository, IUserRepository userRepository, ILikeRepository likeRepositor)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _likeRepositor = likeRepositor;
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
                //NrComments = 0,
                AuthorId = model.AuthorId,
            };
            var response = await _postRepository.CreatePostAsync(post);
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

        public async Task<ToggledLikeResponse> ToggleLikeAsync(ToggleLikeDto model)
        {
            var response = await _likeRepositor.ToggleLikeAsync(model);
            if(response.ErrorCode == Domain.Enum.ErrorCode.Internal_error)
            {
                return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Like attribution failed"};
            }
            return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, Post = response.Post, IsLiked = response.IsLiked };
        }
    }
}
