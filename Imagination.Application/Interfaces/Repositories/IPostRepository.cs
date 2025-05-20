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
    public interface IPostRepository
    {
        Task AddPostAsync(Post post);
        Task<List<Post>> GetAllPostsAsync();
        Task<Post?> GetPostByIdAsync(int postId);
        Task<List<Post>> GetAllPostsByAuthorId(int authorId);
        Task DeletePostAsync(int postId);
        Task UpdatePostAsync(Post post);
    }
}
