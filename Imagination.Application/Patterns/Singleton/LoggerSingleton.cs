using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Imagination.Application.Patterns.Singleton
{
    public sealed class LoggerSingleton
    {
        private static LoggerSingleton _instance;
        private static readonly object _lock = new object();
        private readonly ILogger _logger;

        private LoggerSingleton(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggerSingleton>();
        }

        public static LoggerSingleton GetInstance(ILoggerFactory loggerFactory)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new LoggerSingleton(loggerFactory);
                    }
                }
            }
            return _instance;
        }

        public ILogger GetLogger()
        {
            return _logger;
        }
    }
}
