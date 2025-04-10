using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Dashboard.Commands.CreatePost
{
    public sealed record CreatePostCommand(CreatePostDto model) : ICommand<BaseResponse>;
    internal sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, BaseResponse>
    {
        private readonly IPostService _postService;
        public CreatePostCommandHandler(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<BaseResponse> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            return await _postService.CreatePostAsync(command.model);

        }
    }
}
