using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<ToggledLikeResponse> AddLikeAsync(ToggleLikeDto model);
    }
}
