using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Patterns.ChainOfResponsability;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Infrastructure.Handlers.Comments
{
    public class ParentCommentHandler : CommentHandler
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ParentCommentHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<CreatedCommentResponse> HandleAsync(CreateCommentDto model, Post post)
        {
            if (model.ParentCommentId is null)
            {
                await _commentRepository.AddCommentAsync(new Comment
                {
                    Content = model.Content,
                    DateOfCreation = DateTime.Now,
                    UserId = model.UserId,
                    PostId = model.PostId,
                    ParentCommentId = null,
                });
                post.NrComments++;

                await _unitOfWork.SaveChangesAsync();
                return new CreatedCommentResponse { ErrorCode = Domain.Enum.ErrorCode.NoError, NrComments = post.NrComments };
            }

            return _successor != null
               ? await _successor.HandleAsync(model, post)
               : new CreatedCommentResponse { ErrorCode = Domain.Enum.ErrorCode.Internal_error, ErrorMessage = "Unhandled comment type" };
        }
    }
}
