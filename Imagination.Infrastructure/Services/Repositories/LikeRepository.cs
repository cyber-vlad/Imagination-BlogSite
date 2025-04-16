using Imagination.Application.DTOs;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using Imagination.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ToggledLikeResponse> AddLikeAsync(ToggleLikeDto model)
        {
            try
            {
                var resultLike = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == model.UserId && l.PostId == model.PostId);
                var resultPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == model.PostId);

                if(resultLike == null)
                {
                    await _context.Likes.AddAsync(new Like {UserId = model.UserId, PostId = model.PostId});
                    resultPost.NrLikes++;

                    await _context.SaveChangesAsync();
                    return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, Post = resultPost, IsLiked = true };
                }
                else
                {
                    _context.Likes.Remove(resultLike);
                    resultPost.NrLikes--; 
                    
                    await _context.SaveChangesAsync();
                    return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, Post = resultPost, IsLiked = false };
                }
            }
            catch (Exception ex)
            {
                return new ToggledLikeResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Attribution like failed" };
            }
        }
    }
}
