using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggerLite;
using NSubstitute;
using Xunit;
using ErrorHandling;

namespace ErrorHandling.Test
{
    public class CollectAndIgnoreExceptionPolicyTest
    {
        [Fact]
        public void CtorCreatesValidObject()
        {
            var tested = new CollectAndIgnoreExceptionPolicy();
            Assert.Null(tested.LastException);
            Assert.NotNull(tested.Exceptions);
            Assert.True(tested.Exceptions.IsEmpty);
            Assert.True(tested.StoreLastNExceptions > CollectAndIgnoreExceptionPolicy.MinNExceptions);
            Assert.True(CollectAndIgnoreExceptionPolicy.MinNExceptions < Math.Pow(2, 8));
        }
        [Fact]
        public void StoreLastNExceptionsSetterValidates()
        {
            var tested = new CollectAndIgnoreExceptionPolicy { StoreLastNExceptions = 1 };
            Assert.NotEqual(1, tested.StoreLastNExceptions);
            Assert.Equal(CollectAndIgnoreExceptionPolicy.MinNExceptions, tested.StoreLastNExceptions);
        }

        [Theory]
        [InlineData(typeof(DllNotFoundException))]
        [InlineData(typeof(MemberAccessException))]
        [InlineData(typeof(ArrayTypeMismatchException))]
        public void HandleExceptionStoresLastExceptionAndAddsToCollection(Type exceptionToExcludeType)
        {
            var tested = new CollectAndIgnoreExceptionPolicy();
            var exception = (Exception)Activator.CreateInstance(exceptionToExcludeType);
            tested.HandleException(exception);

            Assert.Equal(exceptionToExcludeType, tested.LastException.GetType());
            Assert.False(tested.Exceptions.IsEmpty);
            Assert.Equal(1, tested.Exceptions.Count);
            tested.Exceptions.TryPeek(out Exception received);
            Assert.Equal(exceptionToExcludeType, received.GetType());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void LastNExceptionsReturnsLastNExceptions(int lastN)
        {
            var tested = new CollectAndIgnoreExceptionPolicy();
            tested.HandleException(new DllNotFoundException());
            tested.HandleException(new Exception());
            tested.HandleException(new MemberAccessException());
            tested.HandleException(new ArrayTypeMismatchException());

            Assert.True(lastN <= CollectAndIgnoreExceptionPolicy.MinNExceptions);
            // Act
            var lastNExcp = tested.LastNExceptions(lastN);

            Assert.Equal(4,tested.Exceptions.Count);
            Assert.Equal(typeof(ArrayTypeMismatchException), tested.LastException.GetType());

            Assert.Equal(lastN, lastNExcp.Count());
            Assert.Equal(typeof(ArrayTypeMismatchException), tested.LastException.GetType());

        }
    }
}
