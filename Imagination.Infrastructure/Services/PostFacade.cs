using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Patterns.ChainOfResponsability;
using Imagination.Application.Patterns.Facade;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using Imagination.Domain.Enum;
using Imagination.Infrastructure.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services
{
    public class PostFacade : IPostFacade
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentHandler _commentHandler;

        public PostFacade(IPostRepository postRepository, IUserRepository userRepository, ILikeRepository likeRepository, ICommentRepository commentRepository, IUnitOfWork unitOfWork, ICommentHandler commentHandler)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _commentHandler = commentHandler;
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

            try
            {
                await _postRepository.AddPostAsync(post);
                return new BaseResponse { ErrorCode = ErrorCode.NoError };
            }
            catch (Exception ex)
            {
                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.Create_post_failed, ErrorMessage = "Create post failed" };
            }
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
                    Author = new AuthorDto
                    {
                        Username = author.Username,
                        PhotoUrl = author.PhotoUrl
                    },
                    IsLikedByCurrentUser = post.Likes?.Any(l => l.UserId == currentUserId) ?? false
                });
            }

            return postDtos;
        }

        public async Task<PostDto> GetPostDetailsByIdAsync(int postId, int currentUserId)
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
                Author = new AuthorDto
                {
                    Username = authorPost.Username,
                    PhotoUrl= authorPost.PhotoUrl,
                },
                Comments = result.Comments?
                    .Select(MapCommentToDto)
                    .ToList(),
                IsLikedByCurrentUser = result.Likes?.Any(l => l.UserId == currentUserId) ?? false
            };

            return post;
        }
        private CommentDto MapCommentToDto(Comment comment)
        {
            if (comment is null)
                return null;

            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                DateOfCreation = comment.DateOfCreation,
                Author = new AuthorDto
                {
                    Username = comment.User.Username,
                    PhotoUrl = comment.User.PhotoUrl,
                },
                ParentCommentId = comment.ParentCommentId,
                Replies = comment.Replies?.Where(r => r != null).Select(MapCommentToDto).ToList() ?? new List<CommentDto>()
            };
        }

        public async Task<ToggledLikeResponse> ToggleLikeAsync(ToggleLikeDto model)
        {
            try
            {
                var resultLike = await _likeRepository.GetLikeByPostUserId(model.PostId, model.UserId);
                var resultPost = await _postRepository.GetPostByIdAsync(model.PostId);

                if(resultLike is null)
                {
                    await _likeRepository.AddLikeAsync(new Like { UserId = model.UserId, PostId = model.PostId });
                    resultPost.NrLikes++;

                    await _unitOfWork.SaveChangesAsync();
                    return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, NrLikes = resultPost.NrLikes, IsLiked = true };
                }
                else
                {
                    await _likeRepository.RemoveLikeAsync(resultLike);
                    resultPost.NrLikes--;

                    await _unitOfWork.SaveChangesAsync();
                    return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, NrLikes = resultPost.NrLikes, IsLiked = false };
                }
            }
            catch (Exception ex)
            {
                return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Like attribution failed" };
            }
        }

        public async Task<CreatedCommentResponse> CreateCommentAsync(CreateCommentDto model)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(model.PostId);
                
                if (post is null)
                    return new CreatedCommentResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Post is not found" };

                return await _commentHandler.HandleAsync(model, post);
            }
            catch(Exception ex)
            {
                return new CreatedCommentResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Creating comment failed" };
            }
        }

        public async Task<List<PostDto>> GetAllPostsByAuthorIdAsync(int authorId)
        {
            try
            {
                var postsReceived = await _postRepository.GetAllPostsByAuthorId(authorId);
                
                if (postsReceived is null) return null;
                
                var posts = new List<PostDto>();
                foreach(var post in postsReceived)
                {
                    posts.Add(new PostDto
                    {
                        PostId = post.Id,
                        Content = post.Content,
                        Title = post.Title,
                        Category = post.Category,
                        DateOfCreation = post.DateOfCreation,
                        NrComments = post.NrComments,
                        NrLikes = post.NrLikes,
                    });
                }

                return posts;
            }
            catch(Exception ex)
            {
                return new List<PostDto>();
            }
        }

        public async Task<BaseResponse> DeletePostAsync(int postId)
        {
            try
            {
                await _postRepository.DeletePostAsync(postId);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, ErrorMessage = "Deleting post failed" };
            }
            catch(Exception ex)
            {
                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.Delete_post_failed, ErrorMessage = "Deleting post failed" };
            }
        }

        public async Task<PostDto> GetAuthorPostByIdAsync(int postId, int authorId)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(postId);
                if (post is not null && post.AuthorId == authorId)
                {
                    return new PostDto
                    {
                        PostId = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        DateOfCreation = post.DateOfCreation,
                        Category = post.Category,
                    };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<BaseResponse> EditPostAsync(EditPostDto model)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(model.PostId);
                post.Title = model.Title;
                post.Content = model.Content;
                post.Category = model.Category;

                await _postRepository.UpdatePostAsync(post);

                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.NoError };

            }
            catch (Exception ex)
            {
                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Editing post failed" };
            }
        }
    }
}
