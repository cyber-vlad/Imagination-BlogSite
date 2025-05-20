using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Patterns.Facade
{
    public interface IPostFacade
    {
        Task<BaseResponse> CreatePostAsync(CreatePostDto model);
        Task<CreatedCommentResponse> CreateCommentAsync(CreateCommentDto model);
        Task<ToggledLikeResponse> ToggleLikeAsync(ToggleLikeDto model);
        Task<List<PostDto>> GetAllPostsForCurrentUserAsync(int currentUserId);
        Task<PostDto> GetPostDetailsByIdAsync(int postId, int currentUserId);
        Task<List<PostDto>> GetAllPostsByAuthorIdAsync(int authorId);
        Task<PostDto> GetAuthorPostByIdAsync(int postId, int authorId);
        Task<BaseResponse> DeletePostAsync(int postId);
        Task<BaseResponse> EditPostAsync(EditPostDto model);
    }
}
