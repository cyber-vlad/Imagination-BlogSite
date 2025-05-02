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
    public interface ICommentRepository
    {
        Task AddCommentAsync(Comment comment);
        Task<Comment?> GetCommentByIdAsync(int commentId);
        
    }
}
