using Azure;
using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using Imagination.Persistence.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse> EditProfileImageAsync(PhotoUserDto model)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                var uploadsPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Uploads");

                var currentUser = await _userRepository.GetUserByIdAsync(model.IdUser);
                if (!string.IsNullOrEmpty(currentUser.PhotoUrl))
                {
                    var oldFilePath = Path.Combine(uploadsPath, currentUser.PhotoUrl.Replace("~/Uploads/", ""));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var fileStoragePath = Path.Combine(uploadsPath, fileName);
                using (var stream = new FileStream(fileStoragePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                currentUser.PhotoUrl = "~/Uploads/" + fileName;
                await _userRepository.UpdateAsync(currentUser);

                List<Claim> userClaims = new List<Claim>();
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, currentUser.Username));
                userClaims.Add(new Claim(ClaimTypes.Email, currentUser.Email));
                userClaims.Add(new Claim("PhotoUrl", currentUser.PhotoUrl));
                userClaims.Add(new Claim("UserRole", currentUser.UserRole.ToString()));

                var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    RedirectUri = "/Profile/Index"
                };

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, ErrorMessage="Successfully"};

            }
            catch (Exception ex)
            {
                return new BaseResponse { ErrorCode = Domain.Enum.ErrorCode.Upload_image_failed, ErrorMessage = "Upload image failed" };
            }
        }
    }
}
