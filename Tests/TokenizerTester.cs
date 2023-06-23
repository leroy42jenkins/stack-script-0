using StackScript0.Core;

namespace StackScript0.Tests
{
    public class TokenizerTester : BaseTester
    {
        private static char sid = Grammer.StringIdentifier;

        public TokenizerTester(bool verbose = true)
        {
            Name = "Tokenizer";
            Verbose = verbose;

            TestList = new List<Action>
            {
                StringComparison0,
                StringComparison1,
                StringComparison2,
                StringComparison3,
                TableComparison0,
                ViewComparison0,
                ClearComparison0,
                DoComparison0,
                IfComparison0,
                BranchComparison0,
                WhileComparison0,
                FibonacciComparison0
            };
        }

        private void StringComparison0() =>
            TokenizedComparison(
                "String Comparison 0",
                $" {sid}test{sid} ",
                new List<Token>
                {
                    new VariableString("test")
                });

        private void StringComparison1() =>
            TokenizedComparison(
                "String Comparison 1",
                $" {sid}  \\{sid}test\\{sid}  {sid} ",
                new List<Token>
                {
                    new VariableString($"  \\{sid}test\\{sid}  ")
                });

        private void StringComparison2() =>
            TokenizedComparison(
                "String Comparison 2",
                $" {sid}\\{sid}test\\\\{sid} ",
                new List<Token>
                {
                    new VariableString($"\\{sid}test\\\\")
                });

        private void StringComparison3() =>
            TokenizedComparison(
                "String Comparison 3",
                $" {sid} \\{sid}test\\ \\{sid}{sid} ",
                new List<Token>
                {
                    new VariableString($" \\{sid}test\\ \\{sid}")
                });

        private void TableComparison0() =>
            TokenizedComparison(
                "Table Comparison 0",
                $"{Grammer.ExpBegin} {sid}one{sid} 1 {Grammer.ExpEnd} {Grammer.TokenTable}",
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("one"),
                    new VariableInt(1),
                    new Token(TokenType.EXPEND),
                    new Word(Grammer.TokenTable)
                });

        private void ViewComparison0() =>
            TokenizedComparison(
                "View Comparison 0",
                $"0 {Grammer.ViewStack} 1 {Grammer.ViewDefinitions} 2",
                new List<Token>
                {
                    new VariableInt(0),
                    new Word(Grammer.ViewStack),
                    new VariableInt(1),
                    new Word(Grammer.ViewDefinitions),
                    new VariableInt(2),
                });

        private void ClearComparison0() =>
            TokenizedComparison(
                "Clear Comparison 0",
                $"0 {Grammer.ClearStack} 1 {Grammer.ClearDefinitions} 2",
                new List<Token>
                {
                    new VariableInt(0),
                    new Word(Grammer.ClearStack),
                    new VariableInt(1),
                    new Word(Grammer.ClearDefinitions),
                    new VariableInt(2),
                });

        private void DoComparison0() =>
            TokenizedComparison(
                "Do Comparison 0",
                $"{Grammer.ExpBegin} 1 1 {Grammer.Add} {Grammer.ExpEnd} {Grammer.ExpDo}",
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableInt(1),
                    new VariableInt(1),
                    new Word(Grammer.Add),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPDO)
                });

        private void IfComparison0() =>
            TokenizedComparison(
                "If Comparison 0",
                $"{Grammer.ExpBegin}1 2 {Grammer.Lower}{Grammer.ExpEnd}" +
                $"{Grammer.ExpBegin}1 1 {Grammer.Add}{Grammer.ExpEnd} {Grammer.ExpIf}",
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableInt(1),
                    new VariableInt(2),
                    new Word(Grammer.Lower),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPBEGIN),
                    new VariableInt(1),
                    new VariableInt(1),
                    new Word(Grammer.Add),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPIF)
                });

        private void BranchComparison0()
        {
            var lines = new List<string>
            {
                $"{Grammer.ExpBegin} 2 2 {Grammer.Equal} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin} {sid}Equal!{sid} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin} {sid}Not Equal.{sid} {Grammer.ExpEnd}",
                $"{Grammer.ExpBranch}"
            };
            var source = string.Join(" ", lines);

            TokenizedComparison(
                "Branch Comparison 0",
                source,
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableInt(2),
                    new VariableInt(2),
                    new Word(Grammer.Equal),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("Equal!"),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("Not Equal."),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPBRANCH)
                });
        }

        private void WhileComparison0()
        {
            var lines = new List<string>
            {
                $"{Grammer.WordBegin} i 1 {Grammer.WordEnd}",
                $"{Grammer.WordBegin}count 10{Grammer.WordEnd}",
                $"{Grammer.ExpBegin} i count {Grammer.Lower} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin} i 1 {Grammer.Add} {Grammer.WordBegin} i {Grammer.WordEnd} {Grammer.ExpEnd}",
                $"{Grammer.ExpWhile}"
            };
            var source = string.Join(" ", lines);

            TokenizedComparison(
                "While Comparison 0",
                source,
                new List<Token>
                {
                    new Token(TokenType.WORDBEGIN),
                    new Word("i"),
                    new VariableInt(1),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.WORDBEGIN),
                    new Word("count"),
                    new VariableInt(10),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.EXPBEGIN),
                    new Word("i"),
                    new Word("count"),
                    new Word(Grammer.Lower),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPBEGIN),
                    new Word("i"),
                    new VariableInt(1),
                    new Word(Grammer.Add),
                    new Token(TokenType.WORDBEGIN),
                    new Word("i"),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPWHILE)
                });
        }

        private void FibonacciComparison0()
        {
            var lines = new List<string>
            {
                $"0 1",
                $"{Grammer.WordBegin} i 0 {Grammer.WordEnd}",
                $"{Grammer.WordBegin} index 5 {Grammer.WordEnd}",
                $"{Grammer.ExpBegin} i index {Grammer.Lower} {Grammer.ExpEnd}",
                $"{Grammer.ExpBegin}",
                $"{Grammer.Dup} 2 {Grammer.Rot} {Grammer.Add}",
                $"i 1 {Grammer.Add} {Grammer.WordBegin} i {Grammer.WordEnd}",
                $"{Grammer.ExpEnd}",
                $"{Grammer.ExpWhile}"
            };
            var source = string.Join(Environment.NewLine, lines);

            TokenizedComparison(
                "Fibonacci Comparison 0",
                source,
                new List<Token>
                {
                    // Fibonnaci index values 0 and 1
                    new VariableInt(0),
                    new VariableInt(1),

                    // while loop values
                    new Token(TokenType.WORDBEGIN),
                    new Word("i"),
                    new VariableInt(0),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.WORDBEGIN),
                    new Word("index"),
                    new VariableInt(5),
                    new Token(TokenType.WORDEND),

                    // while loop test
                    new Token(TokenType.EXPBEGIN),
                    new Word("i"),
                    new Word("index"),
                    new Word(Grammer.Lower),
                    new Token(TokenType.EXPEND),

                    // while loop body
                    new Token(TokenType.EXPBEGIN),
                    // modify top two values of stack
                    new Word(Grammer.Dup),
                    new VariableInt(2),
                    new Word(Grammer.Rot),
                    new Word(Grammer.Add),
                    // increment i
                    new Word("i"),
                    new VariableInt(1),
                    new Word(Grammer.Add),
                    new Token(TokenType.WORDBEGIN),
                    new Word("i"),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.EXPEND),

                    // while loop word
                    new Token(TokenType.EXPWHILE)
                });
        }

        #region Helpers

        private void TokenizedComparison(string name, string source, List<Token> expected)
        {
            var segments = Tokenizer.SourceToSegments(source);
            var actual = Tokenizer.SegmentsToTokens(segments);

            if (actual.Count != expected.Count)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual count of {actual.Count} " +
                        $"not equal to expected count of {expected.Count}");
                }

                Failures += 1;
                return;
            }

            for (var i = 0; i < actual.Count; ++i)
            {
                if (actual[i].IsEqual(expected[i])) continue;

                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual token of {actual[i].ToStringToken(0)} " +
                        $"not equal to expected token of {expected[i].ToStringToken(0)}");
                }

                Failures += 1;
                return;
            }

            if (Verbose)
            {
                PrintSuccess($"{name} test passed");
            }

            Successes += 1;
        }

        #endregion Helpers
    }
}
