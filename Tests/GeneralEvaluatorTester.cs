using StackScript0.Core;

namespace StackScript0.Tests
{
    public class GeneralEvaluatorTester : BaseTester
    {
        private const char sid = Grammer.StringIdentifier;

        private static readonly string testFileMessage = $"1 1 {Grammer.Add} \\{sid}string\\{sid}";
        private static readonly string testFileDir = Path.GetDirectoryName(Environment.CurrentDirectory);
        private static readonly string testFileName = Path.Combine(testFileDir, "test.txt");

        public GeneralEvaluatorTester(bool verbose = true)
        {
            Name = "Stacker";
            Verbose = verbose;

            TestList = new List<Action>
            {
                EvaluateNotEqual0,
                EvaluateType0,
                EvaluateFileWriteActions0,
                EvaluateFileReadActions0,
                EvaluateFileRemovalActions0,
                EvaluateDo0,
                EvaluateDo1,
                EvaluateIf0,
                EvaluateBranch0,
                EvaluateBranch1,
                EvaluateBranch2,
                EvaluateWhile0,
                EvaluateTable0,
                EvaluateTable1,
                EvaluateTable2,
                EvaluateTable3,
                EvaluateTable4,
                () => EvaluateIterativeFibonacci(0, 0, 1),
                () => EvaluateIterativeFibonacci(1, 1, 1),
                () => EvaluateIterativeFibonacci(2, 1, 2),
                () => EvaluateIterativeFibonacci(3, 2, 3),
                () => EvaluateIterativeFibonacci(4, 3, 5),
                () => EvaluateIterativeFibonacci(5, 5, 8),
                () => EvaluateRecursiveFibonacci(0, 0, 1),
                () => EvaluateRecursiveFibonacci(3, 2, 3),
                () => EvaluateRecursiveFibonacci(5, 5, 8)
            };
        }

        private void EvaluateNotEqual0() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.WORDBEGIN),
                    new Word("g"),
                    new VariableInt(1),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.WORDBEGIN),
                    new Word("t"),
                    new VariableInt(2),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.EXPBEGIN),
                    new Word("g"),
                    new Word("t"),
                    new Word(Grammer.Equal),
                    new Word(Grammer.Not),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPDO)
                },
                new List<Token>
                {
                    new VariableInt(1)
                },
                new Dictionary<string, Word>
                {
                    {
                        "g",
                        new Word(
                            "g",
                            new List<Token>
                            {
                                new VariableInt(1)
                            })
                    },
                    {
                        "t",
                        new Word(
                            "t",
                            new List<Token>
                            {
                                new VariableInt(2)
                            })
                    }
                },
                "Not Equal Comparison 0");

        private void EvaluateType0() =>
            EvaluateBasic(
                new List<Token>
                {
                    new VariableInt(0),
                    new Word(Grammer.Type),
                    new VariableFloat(1.0),
                    new Word(Grammer.Type),
                    new VariableString("string"),
                    new Word(Grammer.Type),
                    new VariableTable(new Dictionary<string, Token>()),
                    new Word(Grammer.Type)
                },
                new List<Token>
                {
                    new VariableString("INT"),
                    new VariableString("FLOAT"),
                    new VariableString("STRING"),
                    new VariableString("TABLE")
                },
                new Dictionary<string, Word>(),
                "Type Comparison 0");

        private void EvaluateFileWriteActions0()
        {
            var name = "File Write";
            var firstName = $"{name} Comparison 0";

            if (File.Exists(testFileName))
                File.Delete(testFileName);

            EvaluateBasic(
                new List<Token>
                {
                    new VariableString(testFileMessage),
                    new VariableString(testFileName),
                    new Word(Grammer.FileOut)
                },
                new List<Token>
                {
                    new VariableString(testFileMessage)
                },
                new Dictionary<string, Word>(),
                firstName);

            var secondName = $"{name} IO Comparison 0";

            if (File.Exists(testFileName))
            {
                var s = File.ReadAllText(testFileName);
                File.Delete(testFileName);
                var t = Grammer.StringToEscapedString(s);

                if (t == testFileMessage)
                {
                    if (Verbose)
                    {
                        PrintSuccess($"{secondName} test passed");
                    }

                    Successes += 1;
                }
                else
                {
                    if (Verbose)
                    {
                        PrintFailure($"{secondName} test failed: " +
                            $"actual string of {t} " +
                            $"not equal to expected string {testFileMessage}");
                    }

                    Failures += 1;
                }
            }
            else
            {
                if (Verbose)
                {
                    PrintFailure($"{secondName} test failed: " +
                        $"{testFileName} left in target directory");
                }

                Failures += 1;
            }
        }

        private void EvaluateFileReadActions0()
        {
            var name = "File Read";
            var firstName = $"{name} Comparison 0";

            var t = Grammer.EscapedStringToString(testFileMessage);

            if (File.Exists(testFileName))
                File.Delete(testFileName);

            File.WriteAllText(testFileName, t);

            EvaluateBasic(
                new List<Token>
                {
                    new VariableString(testFileName),
                    new Word(Grammer.FileIn)
                },
                new List<Token>
                {
                    new VariableString(testFileMessage)
                },
                new Dictionary<string, Word>(),
                name);

            var secondName = $"{name} IO Comparison 0";

            var s = File.ReadAllText(testFileName);
            var u = Grammer.StringToEscapedString(s);

            if (testFileMessage == u)
            {
                if (Verbose)
                {
                    PrintSuccess($"{secondName} test passed");
                }

                Successes += 1;
            }
            else
            {
                if (Verbose)
                {
                    PrintFailure($"{secondName} test failed: " +
                        $"actual string of {u} " +
                        $"not equal to expected string {testFileMessage}");
                }

                Failures += 1;
            }

            if (File.Exists(testFileName))
                File.Delete(testFileName);
        }

        private void EvaluateFileRemovalActions0()
        {
            var name = "File Removal";
            var firstName = $"{name} Comparison 0";

            var t = Grammer.EscapedStringToString(testFileMessage);

            if (File.Exists(testFileName))
                File.Delete(testFileName);

            File.WriteAllText(testFileName, t);

            EvaluateBasic(
                new List<Token>
                {
                    new VariableString(testFileName),
                    new Word(Grammer.FileRem)
                },
                new List<Token>(),
                new Dictionary<string, Word>(),
                firstName);

            var secondName = $"{name} IO Comparison 0";

            if (File.Exists(testFileName))
            {
                File.Delete(testFileName);

                if (Verbose)
                {
                    PrintFailure($"{secondName} test failed: " +
                        $"{testFileName} left in target directory");
                }

                Failures += 1;
            }
            else
            {
                if (Verbose)
                {
                    PrintSuccess($"{secondName} test passed");
                }

                Successes += 1;
            }
        }

        private void EvaluateDo0() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableInt(1),
                    new VariableInt(1),
                    new Word(Grammer.Add),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPDO)
                },
                new List<Token>
                {
                    new VariableInt(2)
                },
                new Dictionary<string, Word>(),
                "Do Comparison 0");

        private void EvaluateDo1() =>
            EvaluateBasic(
                new List<Token>
                {
                    new VariableString(testFileMessage),
                    new Token(TokenType.EXPDO)
                },
                new List<Token>
                {
                    new VariableInt(2),
                    new VariableString("string")
                },
                new Dictionary<string, Word>(),
                "Do Comparison 1");

        private void EvaluateIf0() =>
            EvaluateBasic(
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
                },
                new List<Token>
                {
                    new VariableInt(2)
                },
                new Dictionary<string, Word>(),
                "If Comparison 0");

        private void EvaluateBranch0() =>
            EvaluateNestedBranch(1, 2, "Branch Comparison 0");

        private void EvaluateBranch1() =>
            EvaluateNestedBranch(2, 2, "Branch Comparison 1");

        private void EvaluateBranch2() =>
            EvaluateNestedBranch(3, 2, "Branch Comparison 2");

        private void EvaluateWhile0() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.WORDBEGIN),
                    new Word("i"),
                    new VariableInt(1),
                    new Token(TokenType.WORDEND),
                    new Token(TokenType.WORDBEGIN),
                    new Word("count"),
                    new VariableInt(5),
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
                },
                new List<Token>(),
                new Dictionary<string, Word>
                {
                    {
                        "i",
                        new Word(
                            "i",
                            new List<Token>
                            {
                                new VariableInt(5)
                            })
                    },
                    {
                        "count",
                        new Word(
                            "count",
                            new List<Token>
                            {
                                new VariableInt(5)
                            })
                    }
                },
                "While Comparison 0");

        private void EvaluateTable0() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("key"),
                    new VariableString("value"),
                    new Token(TokenType.EXPEND),
                    new Word(Grammer.TokenTable)
                },
                new List<Token>
                {
                    new VariableTable(
                        new Dictionary<string, Token>
                        {
                            { "key", new VariableString("value") }
                        })
                },
                new Dictionary<string, Word>(),
                "Table Comparison 0");

        private void EvaluateTable1() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("key"),
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("value 0"),
                    new VariableString("value 1"),
                    new Token(TokenType.EXPEND),
                    new Token(TokenType.EXPEND),
                    new Word(Grammer.TokenTable)
                },
                new List<Token>
                {
                    new VariableTable(
                        new Dictionary<string, Token>
                        {
                            {
                                "key",
                                new Expression(
                                    new List<Token>
                                    {
                                        new VariableString("value 0"),
                                        new VariableString("value 1")
                                    })
                            }
                        })
                },
                new Dictionary<string, Word>(),
                "Table Comparison 1");

        private void EvaluateTable2() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("key"),
                    new VariableString("value"),
                    new Token(TokenType.EXPEND),
                    new Word(Grammer.TokenTable),
                    new VariableString("key"),
                    new VariableString("another value"),
                    new Word(Grammer.TokenSet),
                    new VariableString("another key"),
                    new VariableString("value"),
                    new Word(Grammer.TokenSet)
                },
                new List<Token>
                {
                    new VariableTable(
                        new Dictionary<string, Token>
                        {
                            { "key", new VariableString("another value") },
                            { "another key", new VariableString("value") }
                        })
                },
                new Dictionary<string, Word>(),
                "Table Comparison 2");

        private void EvaluateTable3() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("key"),
                    new VariableString("value"),
                    new Token(TokenType.EXPEND),
                    new Word(Grammer.TokenTable),
                    new VariableString("key"),
                    new Word(Grammer.TokenGet)
                },
                new List<Token>
                {
                    new VariableTable(
                        new Dictionary<string, Token>
                        {
                            { "key", new VariableString("value") }
                        }),
                    new VariableString("value")
                },
                new Dictionary<string, Word>(),
                "Table Comparison 3");

        private void EvaluateTable4() =>
            EvaluateBasic(
                new List<Token>
                {
                    new Token(TokenType.EXPBEGIN),
                    new VariableString("key"),
                    new VariableString("value"),
                    new Token(TokenType.EXPEND),
                    new Word(Grammer.TokenTable),
                    new VariableString("key"),
                    new Word(Grammer.TokenHas)
                },
                new List<Token>
                {
                    new VariableTable(
                        new Dictionary<string, Token>
                        {
                            { "key", new VariableString("value") }
                        }),
                    new VariableInt(1)
                },
                new Dictionary<string, Word>(),
                "Table Comparison 4");

        #region Helpers

        private void EvaluateBasic(
            List<Token> tokens,
            List<Token> expectedStackTokens,
            Dictionary<string, Word> expectedDefinitions,
            string name)
        {
            var definitions = new Dictionary<string, Word>();
            var tokenStack = new Stack<Token>();

            GeneralEvaluator.Eval(definitions, tokenStack, tokens);

            if (ValidateStack(tokenStack, expectedStackTokens, name) &&
                ValidateDefinitions(definitions, expectedDefinitions, name))
            {
                if (Verbose)
                {
                    PrintSuccess($"{name} test passed");
                }

                Successes += 1;
            }
        }

        private bool ValidateStack(
            Stack<Token> tokenStack,
            List<Token> expectedStackTokens,
            string name)
        {
            var actual = new List<Token>();

            while (tokenStack.Count > 0)
                actual.Add(tokenStack.Pop());

            actual.Reverse();

            if (actual.Count != expectedStackTokens.Count)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual stack count of {actual.Count} " +
                        $"not equal to expected count of {expectedStackTokens.Count}");
                }

                Failures += 1;
                return false;
            }

            for (var i = 0; i < actual.Count; ++i)
            {
                if (actual[i].IsEqual(expectedStackTokens[i])) continue;

                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual stack token of {actual[i].ToStringToken(0)} " +
                        $"not equal to expected token of {expectedStackTokens[i].ToStringToken(0)}");
                }

                Failures += 1;
                return false;
            }

            return true;
        }

        private bool ValidateDefinitions(
            Dictionary<string, Word> definitions,
            Dictionary<string, Word> expectedDefinitions,
            string name)
        {
            if (definitions.Count != expectedDefinitions.Count)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"actual definition count of {definitions.Count} " +
                        $"not equal to expected count of {expectedDefinitions.Count}");
                }

                Failures += 1;
                return false;
            }

            foreach (var pair in definitions)
            {
                var a = pair.Value;

                if (!expectedDefinitions.ContainsKey(pair.Key))
                {
                    if (Verbose)
                    {
                        PrintFailure($"{name} test failed: " +
                            $"actual token of {a.ToStringToken(0)} not found in dictionary");
                    }

                    Failures += 1;
                    return false;
                }

                var e = expectedDefinitions[pair.Key];

                if (!a.IsEqual(e))
                {
                    if (Verbose)
                    {
                        PrintFailure($"{name} test failed: " +
                            $"actual definition token of {a.ToStringToken(0)} " +
                            $"not equal to expected token of {e.ToStringToken(0)}");
                    }

                    Failures += 1;
                    return false;
                }
            }

            return true;
        }

        private void EvaluateNestedBranch(long a, long b, string name)
        {
            var result = a == b
                ? new VariableString("Equal!")
                : a > b
                ? new VariableString("Higher.")
                : new VariableString("Lower.");

            EvaluateBasic(
                Examples.Examples.NestedBranch(a, b),
                new List<Token>
                {
                    result
                },
                new Dictionary<string, Word>(),
                name);
        }

        private void EvaluateIterativeFibonacci(long index, long a, long b)
        {
            EvaluateBasic(
                Examples.Examples.IterativeFibonacci(index),
                new List<Token>
                {
                    new VariableInt(a),
                    new VariableInt(b)
                },
                new Dictionary<string, Word>
                {
                    {
                        "i",
                        new Word(
                            "i",
                            new List<Token>
                            {
                                new VariableInt(index)
                            })
                    },
                    {
                        "index",
                        new Word(
                            "index",
                            new List<Token>
                            {
                                new VariableInt(index)
                            })
                    }
                },
                $"Fibonacci Iterative Comparison {index}");
        }

        private void EvaluateRecursiveFibonacci(long index, long a, long b)
        {
            EvaluateBasic(
                Examples.Examples.RecursiveFibonacci(index),
                new List<Token>
                {
                    new VariableInt(a),
                    new VariableInt(b)
                },
                new Dictionary<string, Word>
                {
                    {
                        "i",
                        new Word(
                            "i",
                            new List<Token>
                            {
                                new VariableInt(index)
                            })
                    },
                    {
                        "index",
                        new Word(
                            "index",
                            new List<Token>
                            {
                                new VariableInt(index)
                            })
                    },
                    {
                        "fib",
                        new Word(
                            "fib",
                            new List<Token>
                            {
                                // test expression
                                new Token(TokenType.EXPBEGIN),
                                new Word("i"),
                                new Word("index"),
                                new Word(Grammer.Lower),
                                new Token(TokenType.EXPEND),
                                //
                                // body expression
                                new Token(TokenType.EXPBEGIN),
                                new Word("i"),
                                new VariableInt(1),
                                new Word(Grammer.Add),
                                new Token(TokenType.WORDBEGIN),
                                new Word("i"),
                                new Token(TokenType.WORDEND),
                                // modify top two values of stack
                                new Word(Grammer.Dup),
                                new VariableInt(2),
                                new Word(Grammer.Rot),
                                new Word(Grammer.Add),
                                new Word("fib"),
                                //
                                new Token(TokenType.EXPEND),
                                new Token(TokenType.EXPIF)
                            })
                    }
                },
                $"Fibonacci Recursive Comparison {index}");
        }

        #endregion Helpers
    }
}
