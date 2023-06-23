using StackScript0.Core;

namespace StackScript0.Tests
{
    public class VarOpEvaluatorTester : BaseTester
    {
        public VarOpEvaluatorTester(bool verbose = true)
        {
            Name = "Variable Operator Evaluator";
            Verbose = verbose;

            TestList = new List<Action>
            {
                EvaluateToInt0,
                EvaluateToFloat0,
                EvaluateToString0,
                EvaluateNot0,
                EvaluateOr0,
                EvaluateAnd0,
                EvaluateAdd0,
                EvaluateSubtract0,
                EvaluateMultiply0,
                EvaluateDivide0,
                EvaluateModulus0,
                EvaluateHigher0,
                EvaluateLower0,
                EvaluateEqual0
            };
        }

        private void EvaluateToInt0()
        {
            EvaluateUnaryOperation(
                "To Int",
                new List<UnaryVariableContainer>
                {
                    new UnaryVariableContainer(new VariableInt(42), new VariableInt(42)),
                    new UnaryVariableContainer(new VariableFloat(42.0), new VariableInt(42)),
                    new UnaryVariableContainer(new VariableString("42"), new VariableInt(42))
                },
                VarOpEvaluator.ToInt);
        }

        private void EvaluateToFloat0()
        {
            EvaluateUnaryOperation(
                "To Float",
                new List<UnaryVariableContainer>
                {
                    new UnaryVariableContainer(new VariableInt(42), new VariableFloat(42.0)),
                    new UnaryVariableContainer(new VariableFloat(42.0), new VariableFloat(42.0)),
                    new UnaryVariableContainer(new VariableString("42.0"), new VariableFloat(42.0))
                },
                VarOpEvaluator.ToFloat);
        }

        private void EvaluateToString0()
        {
            EvaluateUnaryOperation(
                "To String",
                new List<UnaryVariableContainer>
                {
                    new UnaryVariableContainer(new VariableInt(42), new VariableString("42")),
                    new UnaryVariableContainer(new VariableFloat(42.42), new VariableString("42.42")),
                    new UnaryVariableContainer(new VariableString("42.0"), new VariableString("42.0"))
                },
                VarOpEvaluator.ToString);
        }

        private void EvaluateNot0()
        {
            EvaluateUnaryOperation(
                "Not",
                new List<UnaryVariableContainer>
                {
                    new UnaryVariableContainer(new VariableInt(42), new VariableInt(0)),
                    new UnaryVariableContainer(new VariableString("42"), new VariableString(""))
                },
                VarOpEvaluator.Not);
        }

        private void EvaluateOr0()
        {
            EvaluateBinaryOperation(
                "Or",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(42), new VariableInt(41), new VariableInt(42)),
                    new BinaryVariableContainer(new VariableFloat(41.0), new VariableFloat(42.0), new VariableFloat(42.0)),
                },
                VarOpEvaluator.Or);
        }

        private void EvaluateAnd0()
        {
            EvaluateBinaryOperation(
                "And",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(42), new VariableInt(43), new VariableInt(42)),
                    new BinaryVariableContainer(new VariableFloat(43.0), new VariableFloat(42.0), new VariableFloat(42.0)),
                },
                VarOpEvaluator.And);
        }

        private void EvaluateAdd0()
        {
            EvaluateBinaryOperation(
                "Add",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(21), new VariableInt(21), new VariableInt(42)),
                    new BinaryVariableContainer(new VariableFloat(21.0), new VariableFloat(21.0), new VariableFloat(42.0)),
                    new BinaryVariableContainer(new VariableString("4"), new VariableString("2"), new VariableString("42"))
                },
                VarOpEvaluator.Add);
        }

        private void EvaluateSubtract0()
        {
            EvaluateBinaryOperation(
                "Subtract",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(84), new VariableInt(42), new VariableInt(42)),
                    new BinaryVariableContainer(new VariableFloat(84.0), new VariableFloat(42.0), new VariableFloat(42.0)),
                    new BinaryVariableContainer(new VariableString("412"), new VariableString("1"), new VariableString("42"))
                },
                VarOpEvaluator.Subtract);
        }

        private void EvaluateMultiply0()
        {
            EvaluateBinaryOperation(
                "Multiply",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(6), new VariableInt(7), new VariableInt(42)),
                    new BinaryVariableContainer(new VariableFloat(6.0), new VariableFloat(7.0), new VariableFloat(42.0)),
                },
                VarOpEvaluator.Multiply);
        }

        private void EvaluateDivide0()
        {
            EvaluateBinaryOperation(
                "Divide",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(420), new VariableInt(10), new VariableInt(42)),
                    new BinaryVariableContainer(new VariableFloat(420.0), new VariableFloat(10.0), new VariableFloat(42.0)),
                },
                VarOpEvaluator.Divide);
        }

        private void EvaluateModulus0()
        {
            EvaluateBinaryOperation(
                "Modulus",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(100), new VariableInt(58), new VariableInt(42))
                },
                VarOpEvaluator.Modulus);
        }

        private void EvaluateHigher0()
        {
            EvaluateBinaryOperation(
                "Higher",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(3), new VariableInt(2), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableFloat(3.0), new VariableFloat(2.0), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableString("3"), new VariableString("2"), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableInt(2), new VariableInt(3), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableFloat(2.0), new VariableFloat(3.0), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableString("2"), new VariableString("3"), new VariableInt(0))
                },
                VarOpEvaluator.Higher);
        }

        private void EvaluateLower0()
        {
            EvaluateBinaryOperation(
                "Lower",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(3), new VariableInt(2), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableFloat(3.0), new VariableFloat(2.0), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableString("3"), new VariableString("2"), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableInt(2), new VariableInt(3), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableFloat(2.0), new VariableFloat(3.0), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableString("2"), new VariableString("3"), new VariableInt(1))
                },
                VarOpEvaluator.Lower);
        }

        private void EvaluateEqual0()
        {
            EvaluateBinaryOperation(
                "Equal",
                new List<BinaryVariableContainer>
                {
                    new BinaryVariableContainer(new VariableInt(3), new VariableInt(2), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableFloat(3.0), new VariableFloat(2.0), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableString("3"), new VariableString("2"), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableInt(2), new VariableInt(3), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableFloat(2.0), new VariableFloat(3.0), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableString("2"), new VariableString("3"), new VariableInt(0)),
                    new BinaryVariableContainer(new VariableInt(2), new VariableInt(2), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableFloat(2.0), new VariableFloat(2.0), new VariableInt(1)),
                    new BinaryVariableContainer(new VariableString("2"), new VariableString("2"), new VariableInt(1))
                },
                VarOpEvaluator.Equal);
        }

        #region Helpers

        private class UnaryVariableContainer
        {
            public Token InVar;
            public Token OutVar;

            public UnaryVariableContainer(Token inVar, Token outVar)
            {
                InVar = inVar;
                OutVar = outVar;
            }
        }

        private class BinaryVariableContainer
        {
            public Token LeftVar;
            public Token RightVar;
            public Token OutVar;

            public BinaryVariableContainer(Token leftVar, Token rightVar, Token outVar)
            {
                LeftVar = leftVar;
                RightVar = rightVar;
                OutVar = outVar;
            }
        }

        private void EvaluateUnaryOperation(
            string name,
            List<UnaryVariableContainer> containers,
            Func<Token, Token> op)
        {
            var count = 0;

            foreach (var container in containers)
            {
                var test = op(container.InVar);

                if (test.IsEqual(container.OutVar))
                {
                    if (Verbose)
                    {
                        PrintSuccess($"{name} {count} test passed");
                    }

                    Successes += 1;
                }
                else
                {
                    if (Verbose)
                    {
                        PrintFailure($"{name} {count} test failed: " +
                            $"{test.ToStringToken(0)} not equal to {container.OutVar.ToStringToken(0)} " +
                            $"for input variable: {container.InVar.ToStringToken(0)}");
                    }

                    Failures += 1;
                }

                count += 1;
            }
        }

        private void EvaluateBinaryOperation(
            string name,
            List<BinaryVariableContainer> containers,
            Func<Token, Token, Token> op)
        {
            var count = 0;

            foreach (var container in containers)
            {
                var test = op(container.LeftVar, container.RightVar);

                if (test.IsEqual(container.OutVar))
                {
                    if (Verbose)
                    {
                        PrintSuccess($"{name} {count} test passed");
                    }

                    Successes += 1;
                }
                else
                {
                    if (Verbose)
                    {
                        PrintFailure($"{name} {count} test failed");
                        PrintFailure($"\t{test.ToStringToken(0)} not equal to {container.OutVar.ToStringToken(0)}");
                        PrintFailure($"\tfor left input variable: {container.LeftVar.ToStringToken(0)}");
                        PrintFailure($"\tand right input variable: {container.RightVar.ToStringToken(0)}");
                    }

                    Failures += 1;
                }

                count += 1;
            }
        }

        #endregion Helpers
    }
}
