using Imagination.Application.DTOs;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
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
        private readonly IPostFacade _postFacade;
        public CreatePostCommandHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }
        public async Task<BaseResponse> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            return await _postFacade.CreatePostAsync(command.model);

        }
    }
}
