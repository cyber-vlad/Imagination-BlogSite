using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<Like?> GetLikeByPostUserId(int postId, int userId);
        Task AddLikeAsync(Like like);
        Task RemoveLikeAsync(Like like);
    }
}
