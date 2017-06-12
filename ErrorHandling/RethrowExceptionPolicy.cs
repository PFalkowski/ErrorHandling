using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorHandling
{
    /// <summary>
    /// Class that does nothing comapared to usual exception behaviour. Can be useful though in combination with other exceptions in agregate wrapper, to for example logg the exception and rethrow it in single call. 
    /// </summary>
    public class RethrowExceptionPolicy : IExceptionHandlingPolicy
    {
        public void HandleException(Exception ex)
        {
            throw new ExceptionRethrownWrapper(ex);
        }
    }
}
