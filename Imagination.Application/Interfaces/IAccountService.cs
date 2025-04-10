using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse> LoginAsync(LoginDto model);
        Task<BaseResponse> RegisterAsync(RegisterDto model);
        Task<BaseResponse> LogoutAsync();
        Task<BaseResponse> CheckUSerByEmailAsync(string email);
    }
}
