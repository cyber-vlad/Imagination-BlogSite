﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagination.Application.Responses;
using Imagination.Shared;
namespace Imagination.Application.Patterns.CQRS
{
    public interface ICommandHandler<in TCommand, TResponse>
           where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }


}
