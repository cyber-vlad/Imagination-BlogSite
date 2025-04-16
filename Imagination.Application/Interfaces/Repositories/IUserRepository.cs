using Imagination.Domain.Common.Interfaces;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int idUser);
        Task<User?> GetUserByUsernameAndPassword(string username, string password);
        Task<User?> GetUserByEmail(string email);
        Task AddUserAsync(User user);
        Task UpdateAsync(User user);


    }
}
