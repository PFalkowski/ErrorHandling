using System;
using LoggerLite;

namespace ErrorHandling
{
    public class LogExceptionPolicy : IExceptionHandlingPolicy
    {
        private readonly ILogger _logger;

        public LogExceptionPolicy(ILogger logger)
        {
            _logger = logger;
        }

        public void HandleException(Exception ex)
        {
            _logger.LogError(ex);
        }
    }
}