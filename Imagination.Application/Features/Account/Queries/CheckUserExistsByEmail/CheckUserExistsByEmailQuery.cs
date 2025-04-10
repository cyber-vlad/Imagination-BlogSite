using Imagination.Application.DTOs;
using Imagination.Application.Features.Account.Commands.Login;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Account.Queries.CheckUserExistsByEmail
{
    public sealed record CheckUserExistsByEmailQuery(EmailAddressDto model) : IQuery<BaseResponse>;

    public class CheckUserExistsByEmailQueryHandler : IQueryHandler<CheckUserExistsByEmailQuery, BaseResponse>
    {
        private readonly IAccountService _accountService;
        public CheckUserExistsByEmailQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<BaseResponse> Handle(CheckUserExistsByEmailQuery query, CancellationToken cancellationToken)
        {
            return await _accountService.CheckUSerByEmailAsync(query.model.Email);

        }
    }
}
