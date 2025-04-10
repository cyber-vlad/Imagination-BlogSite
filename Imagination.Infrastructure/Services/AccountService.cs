using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagination.Persistence.Context;
using Imagination.Application.Utils;
using Microsoft.AspNetCore.Http;
using Imagination.Domain.Enum;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Imagination.Shared;
namespace Imagination.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> LoginAsync(LoginDto model)
        {
            var user = await _context.Users.Where(u => (u.Email == model.Username || u.Username == model.Username) && u.Password == CryptoData.HashGen(model.Password)).FirstOrDefaultAsync();

            if (user == null || (!user.Username.Equals(model.Username, StringComparison.Ordinal) && !user.Email.Equals(model.Username, StringComparison.Ordinal)))
            {
                return new BaseResponse { ErrorCode = ErrorCode.Username_or_password_is_incorrect, ErrorMessage = "Username or password are wrong!" };
            }

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("PhotoUrl", user.PhotoUrl),
                new Claim("UserRole", user.UserRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(userClaims, "Cookies");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
            };

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            return new BaseResponse { ErrorCode = ErrorCode.NoError };
        }
        public async Task<BaseResponse> RegisterAsync(RegisterDto model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return new BaseResponse { ErrorCode = ErrorCode.Email_already_used, ErrorMessage = "Email address already used!" };
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = CryptoData.HashGen(model.Password),
                UserRole = UserRole.User,
                PhotoUrl = String.Empty,
            };
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new BaseResponse { ErrorCode = 0 };
        }
        public async Task<BaseResponse> LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                await httpContext.SignOutAsync("Cookies");
            }

            return new BaseResponse { ErrorCode = ErrorCode.NoError };
        }
        public async Task<BaseResponse> CheckUSerByEmailAsync(string email)
        {
            var result = await _context.Users.AnyAsync(u => u.Email == email);
            if(!result)
            {
                return new BaseResponse { ErrorCode = ErrorCode.User_not_found, ErrorMessage = "Email address is not registered" };
            }
            return new BaseResponse { ErrorCode = ErrorCode.NoError};
        }


    }
}
