using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ErrorHandling
{
    public class AggregatedExceptionHandlingPolicy : IExceptionHandlingPolicy
    {
        public ConcurrentDictionary<Type, IExceptionHandlingPolicy> Policies { get; }
        public AggregatedExceptionHandlingPolicy(IEnumerable<IExceptionHandlingPolicy> policies)
        {
            var dictionary = policies?.Where(policy => policy != null).ToDictionary(policy => policy.GetType());
            Policies = new ConcurrentDictionary<Type, IExceptionHandlingPolicy>(dictionary);
        }

        public AggregatedExceptionHandlingPolicy(params IExceptionHandlingPolicy[] policies)
            : this(policies?.AsEnumerable())
        {
        }


        public void HandleException(Exception ex)
        {
            if (Policies == null || Policies.Count < 1)
            {
                return;
            }
            var exceptions = new List<Exception>();
            foreach (var p in Policies)
            {
                try
                {
                    p.Value.HandleException(ex);
                }
                catch (Exception inner)
                {
                    exceptions.Add(inner);
                }
            }
            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}