using Imagination.Application.DTOs;
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

        public async Task AddPostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if(post is not null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
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
                            .ThenInclude(r => r.Replies)
                        .Include(p => p.Comments)
                            .ThenInclude(r => r.User)
                        .Include(p => p.Likes)
                        .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<List<Post>> GetAllPostsByAuthorId(int authorId)
        {
            return await _context.Posts
                        .Where(p => p.AuthorId == authorId)
                        .Include(p => p.Likes)
                        .Include(p => p.Comments)
                        .ToListAsync();
                        
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}
