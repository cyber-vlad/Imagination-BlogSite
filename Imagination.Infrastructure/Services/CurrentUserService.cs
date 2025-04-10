using Imagination.Application.Interfaces;
using Imagination.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Imagination.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            string userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isValidGuid = Int32.TryParse(userId, out int parserUserId);


            if (isValidGuid)
            {
                UserId = parserUserId;
            }

            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            UserRole = Enum.TryParse<UserRole>(httpContextAccessor.HttpContext?.User?.FindFirstValue("UserRole"), out var role) ? role : UserRole.None;
            PhotoUrl = httpContextAccessor.HttpContext?.User?.FindFirstValue("PhotoUrl");

        }

        public int UserId { get; }
        public string Email { get; set; }
        public string Username { get; set; }
        public UserRole UserRole { get; set; }
        public string PhotoUrl { get; set; }
    }
}
