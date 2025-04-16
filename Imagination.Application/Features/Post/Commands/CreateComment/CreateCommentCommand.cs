using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagination.Application.Features.Post.Commands.CreateComment
{
    public sealed record CreateCommentCommand(CreateCommentDto model) : ICommand<CreatedCommentResponse>;

    internal sealed class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, CreatedCommentResponse>
    {
        private readonly IPostService _postService;
        public CreateCommentCommandHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<CreatedCommentResponse> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            return await _postService.CreateCommentAsync(command.model);
        }
    }
}
