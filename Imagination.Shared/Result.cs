using Imagination.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Imagination.Shared
{
    //public class Result<T> : IResult<T>
    //{
    //    public List<string> Messages { get; set; } = new List<string>();
    //    public bool Succeeded { get; set; }
    //    public T Data { get; set; }
    //    public Exception Exception { get; set; }
    //    public int Code { get; set; }

    //    #region Non Async Methods

    //    #region Success Methods
    //    public static Result<T> Success()
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = true
    //        };
    //    }

    //    public static Result<T> Success(string message)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = true,
    //            Messages = new List<string> { message }
    //        };
    //    }

    //    public static Result<T> Success(T data)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = true,
    //            Data = data
    //        };
    //    }

    //    public static Result<T> Success(T data, string message)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = true,
    //            Messages = new List<string> { message },
    //            Data = data
    //        };
    //    }
    //    #endregion
    //    #region Failure Methods
    //    public static Result<T> Failure()
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false
    //        };
    //    }

    //    public static Result<T> Failure(string message)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false,
    //            Messages = new List<string> { message }
    //        };
    //    }

    //    public static Result<T> Failure(List<string> messages)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false,
    //            Messages = messages
    //        };
    //    }

    //    public static Result<T> Failure(T data)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false,
    //            Data = data
    //        };
    //    }

    //    public static Result<T> Failure(T data, string message)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false,
    //            Messages = new List<string> { message },
    //            Data = data
    //        };
    //    }

    //    public static Result<T> Failure(T data, List<string> messages)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false,
    //            Messages = messages,
    //            Data = data
    //        };
    //    }

    //    public static Result<T> Failure(Exception exception)
    //    {
    //        return new Result<T>
    //        {
    //            Succeeded = false,
    //            Exception = exception
    //        };
    //    }
    //    #endregion

    //    #endregion

    //    #region Async Methods

    //    #region Success Methods
    //    public static Task<Result<T>> SuccessAsync()
    //    {
    //        return Task.FromResult(Success());
    //    }

    //    public static Task<Result<T>> SuccessAsync(string message)
    //    {
    //        return Task.FromResult(Success(message));
    //    }

    //    public static Task<Result<T>> SuccessAsync(T data)
    //    {
    //        return Task.FromResult(Success(data));
    //    }

    //    public static Task<Result<T>> SuccessAsync(T data, string message)
    //    {
    //        return Task.FromResult(Success(data, message));
    //    }
    //    #endregion
    //    #region Failure Methods
    //    public static Task<Result<T>> FailureAsync()
    //    {
    //        return Task.FromResult(Failure());
    //    }

    //    public static Task<Result<T>> FailureAsync(string message)
    //    {
    //        return Task.FromResult(Failure(message));
    //    }

    //    public static Task<Result<T>> FailureAsync(List<string> messages)
    //    {
    //        return Task.FromResult(Failure(messages));
    //    }

    //    public static Task<Result<T>> FailureAsync(T data)
    //    {
    //        return Task.FromResult(Failure(data));
    //    }

    //    public static Task<Result<T>> FailureAsync(T data, string message)
    //    {
    //        return Task.FromResult(Failure(data, message));
    //    }

    //    public static Task<Result<T>> FailureAsync(T data, List<string> messages)
    //    {
    //        return Task.FromResult(Failure(data, messages));
    //    }

    //    public static Task<Result<T>> FailureAsync(Exception exception)
    //    {
    //        return Task.FromResult(Failure(exception));
    //    }
    //    #endregion

    //    #endregion

    //}
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can't be accessed");

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }
}
