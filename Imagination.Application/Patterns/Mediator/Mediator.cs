using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Patterns.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await SendInternal<TResponse>(request, cancellationToken);
        }

        public async Task<TResponse> Send<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await SendInternal<TResponse>(request, cancellationToken);
        }

        private async Task<TResponse> SendInternal<TResponse>(object request, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            var interfaceType = requestType.GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(ICommand<>) ||
                     i.GetGenericTypeDefinition() == typeof(IQuery<>))
                );

            if (interfaceType == null)
            {
                throw new InvalidOperationException("Request must implement ICommand<TResponse> or IQuery<TResponse>");
            }

            var handlerType = interfaceType.GetGenericTypeDefinition() == typeof(ICommand<>)
                ? typeof(ICommandHandler<,>).MakeGenericType(requestType, responseType)
                : typeof(IQueryHandler<,>).MakeGenericType(requestType, responseType);

            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {requestType} not found");
            }

            var method = handlerType.GetMethod("Handle");
            if (method == null)
            {
                throw new InvalidOperationException("Handle method not found");
            }

            return await (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken });
        }
    }

}
