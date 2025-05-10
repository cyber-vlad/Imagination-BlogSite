using Imagination.Application.DTOs;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Patterns.ChainOfResponsability
{
    public interface ICommentHandler
    {
        void SetSuccessor(ICommentHandler successor);
        Task<CreatedCommentResponse> HandleAsync(CreateCommentDto model, Post post);
    }
}
