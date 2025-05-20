using Imagination.Application.DTOs;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Post.Commands.EditPost
{
    public sealed record EditPostCommand(EditPostDto model) : ICommand<BaseResponse>;
    internal sealed class EditPostCommandHandler : ICommandHandler<EditPostCommand, BaseResponse>
    {
        private readonly IPostFacade _postFacade;

        public EditPostCommandHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }

        public async Task<BaseResponse> Handle(EditPostCommand command, CancellationToken cancellationToken)
        {
            return await _postFacade.EditPostAsync(command.model);
        }
    }
}
