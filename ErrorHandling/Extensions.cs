using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErrorHandling
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregated"></param>
        /// <returns></returns>
        public static string Summary(this IEnumerable<Exception> aggregated)
        {
            if (aggregated == null) throw new ArgumentNullException(nameof(aggregated));
            var builder = new StringBuilder();
            var exceptionsSummaryBase = new Dictionary<Type, int>();
            foreach (var exception in aggregated)
            {
                var T = exception.GetType();
                if (exceptionsSummaryBase.ContainsKey(T))
                {
                    ++exceptionsSummaryBase[T];
                }
                else
                {
                    exceptionsSummaryBase.Add(T, 1);
                }
            }
            var orderedByValue = exceptionsSummaryBase.OrderByDescending(x => x.Value).ToList();
            if (orderedByValue.Count == 0)
            {
                return "No errors.";
            }
            using (var iter = orderedByValue.GetEnumerator())
            {
                builder.AppendLine(exceptionsSummaryBase.Sum(x => x.Value) > 1
                    ? "Multiple errors occurred:"
                    : "Error occurred:");
                while (iter.MoveNext())
                {
                    builder.AppendLine(
                        $"{iter.Current.Key}: {(iter.Current.Value == 1 ? "once" : $"{iter.Current.Value} times")}");
                }
            }
            return builder.ToString();
        }

        public static string Summary(this AggregateException aggregated)
        {
            if (aggregated == null) throw new ArgumentNullException(nameof(aggregated));
            return Summary(aggregated.InnerExceptions);
        }
    }
}
