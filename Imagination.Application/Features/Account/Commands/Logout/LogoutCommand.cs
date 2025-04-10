using Imagination.Application.Features.Account.Commands.Login;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Account.Commands.Logout
{
    public class LogoutCommand() : ICommand<BaseResponse>;
    internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, BaseResponse>
    {
        private readonly IAccountService _accountService;
        public LogoutCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<BaseResponse> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            return await _accountService.LogoutAsync();

        }
    }
}
