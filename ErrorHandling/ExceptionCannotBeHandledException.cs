using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorHandling
{
    public class ExceptionCannotBeHandledException : Exception
    {
        public const string DefaultMessage = "Exception could not be handled";
        public ExceptionCannotBeHandledException() : base(DefaultMessage) { }
        public ExceptionCannotBeHandledException(Exception innerException) : base(DefaultMessage, innerException) { }
        public ExceptionCannotBeHandledException(string message) : base(message) { }
        public ExceptionCannotBeHandledException(string message, Exception innerException) : base(message, innerException) { }
    }
}
