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
        public async Task<Like?> GetLikeByPostUserId(int postId, int userId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        }

        public async Task AddLikeAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(Like like)
        {
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }
    }
}
