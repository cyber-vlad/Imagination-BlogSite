using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Patterns.CQRS
{

    public interface ICommand : IBaseCommand
    {
    }

    public interface ICommand<out TResponse> : IBaseCommand
    {
    }
    public interface IBaseCommand
    {
    }

}
