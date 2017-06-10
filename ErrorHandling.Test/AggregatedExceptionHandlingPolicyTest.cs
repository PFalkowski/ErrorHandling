using System;
using System.Collections.Generic;
using NSubstitute;
using Xunit;

namespace ErrorHandling.Test
{
    public class AggregatedExceptionHandlingPolicyTest
    {
        [Fact]
        public void CtorCreatesValidObject()
        {
            var tested = new AggregatedExceptionHandlingPolicy();
            Assert.NotNull(tested.Policies);
        }

        [Fact]
        public void CtorCreatesValidObject2()
        {
            var tested = new AggregatedExceptionHandlingPolicy(default(IExceptionHandlingPolicy));
            Assert.NotNull(tested.Policies);
        }

        [Fact]
        public void CtorCreatesValidObject3()
        {
            var policySubstitute = Substitute.For<IExceptionHandlingPolicy>();
            var policies =
                new List<IExceptionHandlingPolicy> { policySubstitute, default(IExceptionHandlingPolicy) };
            var tested = new AggregatedExceptionHandlingPolicy(policies);
            Assert.NotNull(tested.Policies);
            Assert.Equal(1, tested.Policies.Count);
            Assert.Equal(policies[0], policySubstitute);
        }

        [Fact]
        public void HandleExceptionActivatesAllNotNullPoliciesHandleException()
        {
            var exception = new DllNotFoundException();
            var policySubstitute = Substitute.For<IExceptionHandlingPolicy>();
            var policies = new List<IExceptionHandlingPolicy> { policySubstitute, default(IExceptionHandlingPolicy) };
            var tested = new AggregatedExceptionHandlingPolicy(policies);
            Assert.NotNull(tested.Policies);
            Assert.Equal(1, tested.Policies.Count);

            // act

            tested.HandleException(exception);

            // assert

            policySubstitute.Received(1).HandleException(exception);
        }

    }
}
