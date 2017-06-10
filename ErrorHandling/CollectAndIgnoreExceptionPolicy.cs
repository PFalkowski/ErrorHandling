using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ErrorHandling
{
    public class CollectAndIgnoreExceptionPolicy : IExceptionHandlingPolicy
    {
        private const int RetriesMax = 100;
        public const int MinNExceptions = 10;

        private int _storeLastNExceptions = 1000;

        public readonly ConcurrentQueue<Exception> Exceptions = new ConcurrentQueue<Exception>();

        public int StoreLastNExceptions
        {
            get => _storeLastNExceptions;
            set => _storeLastNExceptions = value > MinNExceptions ? value : MinNExceptions;
        }

        public Exception LastException { get; private set; }

        public void HandleException(Exception ex)
        {
            int i = 0;
            while (Exceptions.Count >= StoreLastNExceptions || i > RetriesMax)
            {
                Exceptions.TryDequeue(out Exception _);
                ++i;
            }
            LastException = ex;
            Exceptions.Enqueue(ex);
        }

        public IEnumerable<Exception> LastNExceptions(int n)
        {
            return Exceptions.Take(n);
        }
    }
}