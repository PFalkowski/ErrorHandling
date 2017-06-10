using System;

namespace ErrorHandling
{
    public class IgnoreExceptionPolicy : IExceptionHandlingPolicy
    {
        public void HandleException(Exception ex)
        {
            //omnomnom!
        }
    }
}