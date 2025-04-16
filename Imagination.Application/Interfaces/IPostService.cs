using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces
{
    public interface IPostService
    {
        Task<BaseResponse> CreatePostAsync(CreatePostDto model);
        Task<List<PostDto>> GetAllPostsForCurrentUserAsync(int currentUserId);
        Task<PostDto> GetPostDetailsByIdAsync(int postId);
        Task<ToggledLikeResponse> ToggleLikeAsync(ToggleLikeDto model);
        Task<CreatedCommentResponse> CreateCommentAsync(CreateCommentDto model);
    }
}
