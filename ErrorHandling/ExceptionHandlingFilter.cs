using System;
using System.Collections.Generic;
using System.Threading;

namespace ErrorHandling
{
    public class ExceptionHandlingFilter : IExceptionHandlingPolicy
    {
        private readonly IExceptionHandlingPolicy _wrappedPolicy;

        /// <summary>
        /// Add your unhandlable / filtered out exception types to this HashSet
        /// </summary>
        public readonly HashSet<Type> ExcludedExceptios = new HashSet<Type>
        {
            typeof (OutOfMemoryException),
            typeof(StackOverflowException),
            typeof(ThreadAbortException),
            typeof (InsufficientExecutionStackException)
        };

        public ExceptionHandlingFilter(IExceptionHandlingPolicy wrappedPolicy)
        {
            _wrappedPolicy = wrappedPolicy;
        }

        public void HandleException(Exception ex)
        {
            if (CanBeHandled(ex))
            {
                _wrappedPolicy.HandleException(ex);
            }
            else
            {
                throw new ExceptionCannotBeHandledException(ex); // preserve original exception's stack trace
            }
        }

        public bool CanBeHandled(Exception ex)
        {
            return !CanNotBeHandled(ex);
        }

        public bool CanNotBeHandled(Exception ex)
        {
            return ExcludedExceptios.Contains(ex.GetType());
        }
    }
}