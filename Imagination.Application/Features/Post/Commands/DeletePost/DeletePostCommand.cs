using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagination.Application.Features.Post.Commands.DeletePost
{
    public sealed record DeletePostCommand(int postId) : ICommand<BaseResponse>;
    internal sealed class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, BaseResponse>
    {
        private readonly IPostFacade _postFacade;

        public DeletePostCommandHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }

        public async Task<BaseResponse> Handle(DeletePostCommand command, CancellationToken cancellationToken)
        {
            return await _postFacade.DeletePostAsync(command.postId);
        }
    }
}
