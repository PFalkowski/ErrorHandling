using System;
using Xunit;

namespace ErrorHandling.Test
{
    public class IgnoreExceptionPolicyTest
    {
        [Fact]
        public void CanCreate()
        {
            var tested = new IgnoreExceptionPolicy();
        }
        [Fact]
        public void HandlePolicyDoesNothing()
        {
            var tested = new IgnoreExceptionPolicy();
            var exception = new InvalidOperationException();
            tested.HandleException(exception);
        }
    }
}
