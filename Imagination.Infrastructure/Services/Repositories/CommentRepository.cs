using Imagination.Application.DTOs;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using Imagination.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return new Comment();
        }
    }
}
