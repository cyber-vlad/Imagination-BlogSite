using Imagination.Application.DTOs;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Dashboard.Commands.ToggleLike
{
    public sealed record ToggleLikeCommand(ToggleLikeDto model) : ICommand<ToggledLikeResponse>;
    internal sealed class ToggleLikeCommandHandler : ICommandHandler<ToggleLikeCommand, ToggledLikeResponse>
    {
        private readonly IPostFacade _postFacade;
        public ToggleLikeCommandHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }
        public async Task<ToggledLikeResponse> Handle(ToggleLikeCommand command, CancellationToken cancellationToken)
        {
            return await _postFacade.ToggleLikeAsync(command.model);
        }
    }
}
