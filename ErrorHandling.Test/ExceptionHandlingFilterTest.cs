using NSubstitute;
using System;
using System.Reflection;
using Xunit;

namespace ErrorHandling.Test
{
    public class ExceptionHandlingFilterTest
    {
        [Fact]
        public void CanCreate()
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
        }

        [Fact]
        public void ByDefaultThereAreUnhandlableExceptionsAdded()
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
            Assert.True(filter.ExcludedExceptios.Contains(typeof(OutOfMemoryException)));
            Assert.True(filter.ExcludedExceptios.Contains(typeof(InsufficientExecutionStackException)));
        }

        [Theory]
        [InlineData(typeof(DllNotFoundException))]
        [InlineData(typeof(MemberAccessException))]
        [InlineData(typeof(ArrayTypeMismatchException))]
        public void FilterFiltersOut(Type exceptionToExcludeType)
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
            filter.ExcludedExceptios.Add(exceptionToExcludeType);
            var exception = (Exception)Activator.CreateInstance(exceptionToExcludeType);
            Assert.False(filter.CanBeHandled(exception));
        }

        [Theory]
        [InlineData(typeof(DllNotFoundException))]
        [InlineData(typeof(MemberAccessException))]
        [InlineData(typeof(ArrayTypeMismatchException))]
        public void FilterDoesNotFilterNotAddedExceptions(Type exceptionToExcludeType)
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
            var exception = (Exception)Activator.CreateInstance(exceptionToExcludeType);
            Assert.True(filter.CanBeHandled(exception));
        }

        [Theory]
        [InlineData(typeof(DllNotFoundException))]
        [InlineData(typeof(MemberAccessException))]
        [InlineData(typeof(ArrayTypeMismatchException))]
        public void FilterPassesExceptionHandlingToUnderlying(Type exceptionToExcludeType)
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
            var exception = (Exception)Activator.CreateInstance(exceptionToExcludeType);
            
            filter.HandleException(exception);

            substituteForErrorHandlingPolicy.Received(1).HandleException(Arg.Is(exception));
        }

        [Theory]
        [InlineData(typeof(DllNotFoundException))]
        [InlineData(typeof(MemberAccessException))]
        [InlineData(typeof(ArrayTypeMismatchException))]
        public void FilterDoesNotPassExceptionHandlingToUnderlyingIfCannotBeHandled(Type exceptionToExcludeType)
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
            filter.ExcludedExceptios.Add(exceptionToExcludeType);
            var exception = (Exception)Activator.CreateInstance(exceptionToExcludeType);

            
            Assert.Throws<ExceptionCannotBeHandledException>(() => filter.HandleException(exception));
            substituteForErrorHandlingPolicy.DidNotReceiveWithAnyArgs().HandleException(Arg.Any<Exception>());
        }

        [Theory]
        [InlineData(typeof(DllNotFoundException))]
        [InlineData(typeof(MemberAccessException))]
        [InlineData(typeof(ArrayTypeMismatchException))]
        public void FilterThrowsExceptionWrappedIfItCannotBeHandled(Type exceptionToExcludeType)
        {
            var substituteForErrorHandlingPolicy = Substitute.For<IExceptionHandlingPolicy>();
            var filter = new ExceptionHandlingFilter(substituteForErrorHandlingPolicy);
            filter.ExcludedExceptios.Add(exceptionToExcludeType);
            var exception = (Exception)Activator.CreateInstance(exceptionToExcludeType);

            Assert.False(filter.CanBeHandled(exception));
            var rec = Assert.Throws<ExceptionCannotBeHandledException>(() => filter.HandleException(exception));
            Assert.True(rec.InnerException.GetType() == exceptionToExcludeType);
            Assert.Equal(rec.InnerException, exception);
        }
    }
}
