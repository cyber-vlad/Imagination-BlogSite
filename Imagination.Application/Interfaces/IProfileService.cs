using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces
{
    public interface IProfileService
    {
        Task<BaseResponse> EditProfileImageAsync(PhotoUserDto model);
    }
}