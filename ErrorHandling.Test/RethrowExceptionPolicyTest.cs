using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ErrorHandling.Test
{
    public class RethrowExceptionPolicyTest
    {


        [Fact]
        public void HandleExceptionRethrowsExceptionsWrappedInAggregate()
        {
            var exception = new DllNotFoundException();
            var tested = new RethrowExceptionPolicy();

            // act & assert
            var ex = Assert.Throws<ExceptionRethrownWrapper>(() => tested.HandleException(exception));
            Assert.IsType(exception.GetType(), ex.InnerException);
        }
    }
}
