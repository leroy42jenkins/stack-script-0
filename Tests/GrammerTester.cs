using StackScript0.Core;
using System.Xml.Linq;

namespace StackScript0.Tests
{
    internal class GrammerTester : BaseTester
    {
        private char sid = Grammer.StringIdentifier;

        public GrammerTester(bool verbose = true)
        {
            Name = "Grammer";
            Verbose = verbose;

            TestList = new List<Action>
            {
                StringToEscapedString0,
                EscapedStringToString0
            };
        }

        private void StringToEscapedString0() =>
            GrammerStringComparison(
                "String to Escaped-String Comparison 0",
                Grammer.StringToEscapedString,
                $"1 1 {Grammer.Add} {sid}string{sid}",
                $"1 1 {Grammer.Add} \\{sid}string\\{sid}");

        private void EscapedStringToString0() =>
            GrammerStringComparison(
                "Escaped-String To String Comparison 0",
                Grammer.EscapedStringToString,
                $"1 1 {Grammer.Add} \\{sid}string\\{sid}",
                $"1 1 {Grammer.Add} {sid}string{sid}");

        #region Helpers

        private void GrammerStringComparison(
            string name,
            Func<string, string> f,
            string source,
            string expected)
        {
            var actual = f(source);

            if (actual != expected)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual string of {actual} " +
                        $"not equal to expected string of {expected}");
                }

                Failures += 1;
            }
            else
            {
                if (Verbose)
                {
                    PrintSuccess($"{name} test passed");
                }

                Successes += 1;
            }
        }

        private void GrammerGeneralComparison(
            string name,
            Token actual,
            Token expected)
        {

            if (actual.IsEqual(expected))
            {
                if (Verbose)
                {
                    PrintSuccess($"{name} test passed");
                }

                Successes += 1;
            }
            else
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual double of {actual.ToStringToken(0)} " +
                        $"not equal to expected double of {expected.ToStringToken(0)}");
                }

                Failures += 1;
            }
        }

        #endregion Helpers
    }
}
