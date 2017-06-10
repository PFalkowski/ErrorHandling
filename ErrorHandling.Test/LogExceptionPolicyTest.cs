using System;
using LoggerLite;
using NSubstitute;
using Xunit;

namespace ErrorHandling.Test
{
    public class LogExceptionPolicyTest
    {
        [Fact]
        public void CanCreate()
        {
            var loggerMock = Substitute.For<ILogger>();
            var tested = new LogExceptionPolicy(loggerMock);
        }
        [Fact]
        public void HandlePolicyLogsTheException()
        {
            var loggerMock = Substitute.For<ILogger>();
            var tested = new LogExceptionPolicy(loggerMock);
            var exception = new InvalidOperationException();
            tested.HandleException(exception);
            loggerMock.Received(1).LogError(Arg.Is<InvalidOperationException>(x => x == exception));
        }
        [Fact]
        public void HandlePolicyIgnoresNullArgument()
        {
            var loggerMock = Substitute.For<ILogger>();
            var tested = new LogExceptionPolicy(loggerMock);
            var exception = default(InvalidOperationException);
            tested.HandleException(exception);
            loggerMock.Received(1).LogError(Arg.Is<InvalidOperationException>(x => x == null));
        }
    }
}
