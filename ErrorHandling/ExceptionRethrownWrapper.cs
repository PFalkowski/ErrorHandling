using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorHandling
{
    public class ExceptionRethrownWrapper : Exception
    {
        public const string DefaultMessage = "Exception rethrown - check inner exception for details.";
        public ExceptionRethrownWrapper() : base(DefaultMessage) { }
        public ExceptionRethrownWrapper(Exception innerException) : base(DefaultMessage, innerException) { }
        public ExceptionRethrownWrapper(string message) : base(message) { }
        public ExceptionRethrownWrapper(string message, Exception innerException) : base(message, innerException) { }
    }
}
