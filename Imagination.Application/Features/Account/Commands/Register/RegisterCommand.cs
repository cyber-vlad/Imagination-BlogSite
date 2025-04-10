using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagination.Application.Features.Account.Commands.Register
{
    public sealed record RegisterCommand(RegisterDto model) : ICommand<BaseResponse>;
    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, BaseResponse>
    {
        private readonly IAccountService _accountService;
        public RegisterCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<BaseResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            return await _accountService.RegisterAsync(command.model);

        }
    }
}
