﻿using Imagination.Application.DTOs;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Responses;
using Imagination.Application.Utils;
using Imagination.Domain.Entities;
using Imagination.Domain.Enum;
using Imagination.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse> AddPostAsync(Post post)
        {
            try
            {
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                return new BaseResponse { ErrorCode = ErrorCode.NoError };
            }
            catch (Exception ex)
            {
                return new BaseResponse { ErrorCode = ErrorCode.Create_post_failed };
            }
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                        .Include(p => p.Likes)    
                        .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }
    }
}
