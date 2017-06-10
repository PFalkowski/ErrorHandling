using System;

namespace ErrorHandling
{
    public interface IExceptionHandlingPolicy
    {
        void HandleException(Exception ex);
    }
}
