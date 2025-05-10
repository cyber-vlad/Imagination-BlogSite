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
    public abstract class CommentHandler : ICommentHandler
    {
        protected ICommentHandler _successor;

        public void SetSuccessor(ICommentHandler successor)
        {
            _successor = successor;
        }

        public abstract Task<CreatedCommentResponse> HandleAsync(CreateCommentDto model, Post post);
    }
}
