using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace ErrorHandling.Test
{
    public class ExtensionsTest
    {
        private const string OpeningSequenceMultipleRegex = "(E|e)rrors.(O|o)ccurred";
        private const string OpeningSequenceSingleRegex = "(E|e)rror.(O|o)ccurred";
        private const string RegexForOnce = ".?\\s?(((O|o)nce)|1 time)";
        private const string RegexForTwice = ".{0,3}2\\s(T|t)imes";
        private const string RegexForThreeTimes = ".{0,3}3\\s(T|t)imes";
        private readonly Regex _noErrorsRegex = new Regex("no.errors?", RegexOptions.IgnoreCase);
        private readonly Regex _oneErrorRegex = new Regex("error occurred", RegexOptions.IgnoreCase);

        [Fact]
        public void SummarySummarizesMultipleErrors()
        {
            var error1 = new DllNotFoundException();
            var error2 = new ArgumentException();
            var error3 = new ArgumentException();
            var error4 = new ArgumentException();
            var error5 = new COMException();

            var exceptionsAggregated = new List<Exception> { error1, error2, error3, error4, error5 };
            var received = exceptionsAggregated.Summary();

            Assert.True(received.Contains(nameof(DllNotFoundException)));
            Assert.True(received.Contains(nameof(ArgumentException)));
            Assert.True(received.Contains(nameof(COMException)));

            Assert.True(Regex.IsMatch(received, OpeningSequenceMultipleRegex));
            Assert.True(Regex.IsMatch(received, $"{nameof(ArgumentException)}{RegexForThreeTimes}"));
            Assert.True(Regex.IsMatch(received, $"{nameof(DllNotFoundException)}{RegexForOnce}"));
            Assert.True(Regex.IsMatch(received, $"{nameof(COMException)}{RegexForOnce}"));
        }
        [Fact]
        public void SummaryOrdersErrorsFromMostOccuringDescending()
        {
            var error1 = new DllNotFoundException();
            var error2 = new ArgumentException();
            var error3 = new ArgumentException();
            var error4 = new ArgumentException();

            var exceptionsAggregated = new List<Exception> { error1, error2, error3, error4 };
            var received = exceptionsAggregated.Summary();
            var multilineRegex = new Regex($"{OpeningSequenceMultipleRegex}.?.?.?({error2.GetType().FullName}|{error2.GetType().Name}){RegexForThreeTimes}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Assert.True(multilineRegex.IsMatch(received));
        }
        [Fact]
        public void SummaryGentlyRespondsWhenNoErrors()
        {
            var received = new List<Exception>().Summary();

            Assert.True(_noErrorsRegex.IsMatch(received));
        }
        [Fact]
        public void SummaryThrowsWhenNullArguiment()
        {
            List<Exception> received = null;

            Assert.Throws<ArgumentNullException>(() => Extensions.Summary(received));
            Assert.Throws<ArgumentNullException>(() => received.Summary());
        }
        [Fact]
        public void SummaryDoesNotPluralizeWhenOnlyOneErrorOccurred()
        {
            var error1 = new COMException();

            var exceptionsAggregated = new List<Exception> { error1 };
            var received = exceptionsAggregated.Summary();

            Assert.True(received.Contains(nameof(COMException)));

            Assert.True(_oneErrorRegex.IsMatch(received));
        }

        [Fact]
        public void SummaryForAggregatedSummarizesMultipleErrors()
        {
            var error1 = new DllNotFoundException();
            var error2 = new ArgumentException();
            var error3 = new ArgumentException();
            var error4 = new ArgumentException();
            var error5 = new COMException();

            var tmp = new List<Exception> { error1, error2, error3, error4, error5 };
            var exceptionsAggregated = new AggregateException(tmp);
            var received = exceptionsAggregated.Summary();

            Assert.True(received.Contains(nameof(DllNotFoundException)));
            Assert.True(received.Contains(nameof(ArgumentException)));
            Assert.True(received.Contains(nameof(COMException)));

            Assert.True(Regex.IsMatch(received, OpeningSequenceMultipleRegex));
            Assert.True(Regex.IsMatch(received, $"{nameof(ArgumentException)}{RegexForThreeTimes}"));
            Assert.True(Regex.IsMatch(received, $"{nameof(DllNotFoundException)}{RegexForOnce}"));
            Assert.True(Regex.IsMatch(received, $"{nameof(COMException)}{RegexForOnce}"));
        }
        [Fact]
        public void SummaryForAggregatedOrdersErrorsFromMostOccuringDescending()
        {
            var error1 = new DllNotFoundException();
            var error2 = new ArgumentException();
            var error3 = new ArgumentException();
            var error4 = new ArgumentException();

            var tmp = new List<Exception> { error1, error2, error3, error4 };
            var exceptionsAggregated = new AggregateException(tmp);
            var received = exceptionsAggregated.Summary();
            var multilineRegex = new Regex($"{OpeningSequenceMultipleRegex}.?.?.?({error2.GetType().FullName}|{error2.GetType().Name}){RegexForThreeTimes}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Assert.True(multilineRegex.IsMatch(received));
        }
        [Fact]
        public void SummaryForAggregatedGentlyRespondsWhenNoErrors()
        {
            var received = new AggregateException().Summary();

            Assert.True(_noErrorsRegex.IsMatch(received));
        }
        [Fact]
        public void SummaryForAggregatedThrowsWhenNullArguiment()
        {
            AggregateException received = null;

            Assert.Throws<ArgumentNullException>(() => Extensions.Summary(received));
            Assert.Throws<ArgumentNullException>(() => received.Summary());
        }
        [Fact]
        public void SummaryForAggregatedDoesNotPluralizeWhenOnlyOneErrorOccurred()
        {
            var error1 = new COMException();

            var exceptionsAggregated = new AggregateException(new List <Exception> { error1 });
            var received = exceptionsAggregated.Summary();

            Assert.True(received.Contains(nameof(COMException)));

            Assert.True(_oneErrorRegex.IsMatch(received));
        }
    }
}
