using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Imagination.Application.Patterns.Mediator.Interfaces
{
    public interface ISender
    {
        Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default);

        //Task<BaseResponse?> Send(object request, CancellationToken cancellationToken = default);

    }
}
