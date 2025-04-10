using Imagination.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imagination.Shared;
using Imagination.Application.Interfaces;
using MediatR;
using Imagination.Application.Responses;
using Imagination.Application.Patterns.CQRS;
namespace Imagination.Application.Features.Account.Commands.Login
{
    public sealed record LoginCommand(LoginDto model) : ICommand<BaseResponse>;
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, BaseResponse>
    {
        private readonly IAccountService _accountService;
        public LoginCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<BaseResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            return await _accountService.LoginAsync(command.model);
            
        }
    }
}
