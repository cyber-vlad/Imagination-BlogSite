using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Utils;
using Imagination.Domain.Common.Interfaces;
using Imagination.Domain.Entities;
using Imagination.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int idUser)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Id == idUser);
        }

        public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.Users.Where(u => (u.Email == username || u.Username == username) && u.Password == password).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
