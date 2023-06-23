using StackScript0.Core;

namespace StackScript0.Tests
{
    public class WordIncStackEvaluatorTester : BaseTester
    { 
        public WordIncStackEvaluatorTester(bool verbose = true)
        {
            Name = "Included-Word Stack Evaluator";
            Verbose = verbose;

            TestList = new List<Action>
            {
                EvaluateNot0,
                EvaluateCastInt0,
                EvaluateCastFloat0,
                EvaluateCastString0,
                EvaluateOr0,
                EvaluateAnd0,
                EvaluateLower0,
                EvaluateEqual0,
                EvaluateHigher0,
                EvaluateAdd0,
                EvaluateSubtract0,
                EvaluateMultiply0,
                EvaluateDivide0,
                EvaluateModulus0,
                EvaluateDup0,
                EvaluatePop0,
                EvaluateRot0,
                EvaluateRot1,
                EvaluateRot2
            };
        }

        private void EvaluateNot0() =>
            EvaluateUnaryOperation(
                $"`{Grammer.Not}` 0", new VariableInt(1), new VariableInt(0),
                WordIncStackEvaluator.Not);

        private void EvaluateCastInt0() =>
            EvaluateUnaryOperation(
                $"`{Grammer.CastInt}` 0", new VariableFloat(1.0), new VariableInt(1),
                WordIncStackEvaluator.CastInt);

        private void EvaluateCastFloat0() =>
            EvaluateUnaryOperation(
                $"`{Grammer.CastFloat}` 0", new VariableInt(1), new VariableFloat(1.0),
                WordIncStackEvaluator.CastFloat);

        private void EvaluateCastString0() =>
            EvaluateUnaryOperation(
                $"`{Grammer.CastString}` 0", new VariableInt(1), new VariableString("1"),
                WordIncStackEvaluator.CastString);

        private void EvaluateOr0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Or}` 0", new VariableInt(1), new VariableInt(0), new VariableInt(1),
                WordIncStackEvaluator.Or);

        private void EvaluateAnd0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.And}` 0", new VariableInt(1), new VariableInt(0), new VariableInt(0),
                WordIncStackEvaluator.And);

        private void EvaluateLower0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Lower}` 0", new VariableInt(1), new VariableInt(0), new VariableInt(0),
                WordIncStackEvaluator.Lower);

        private void EvaluateEqual0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Equal}` 0", new VariableInt(1), new VariableInt(1), new VariableInt(1),
                WordIncStackEvaluator.Equal);

        private void EvaluateHigher0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Higher}` 0", new VariableInt(1), new VariableInt(0), new VariableInt(1),
                WordIncStackEvaluator.Higher);

        private void EvaluateAdd0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Add}` 0", new VariableInt(1), new VariableInt(1), new VariableInt(2),
                WordIncStackEvaluator.Add);

        private void EvaluateSubtract0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Subtract}` 0", new VariableInt(1), new VariableInt(1), new VariableInt(0),
                WordIncStackEvaluator.Subtract);

        private void EvaluateMultiply0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Multiply}` 0", new VariableInt(2), new VariableInt(3), new VariableInt(6),
                WordIncStackEvaluator.Multiply);

        private void EvaluateDivide0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Divide}` 0", new VariableInt(5), new VariableInt(3), new VariableInt(1),
                WordIncStackEvaluator.Divide);

        private void EvaluateModulus0() =>
            EvaluateBinaryOperation(
                $"`{Grammer.Modulus}` 0", new VariableInt(7), new VariableInt(4), new VariableInt(3),
                WordIncStackEvaluator.Modulus);

        private void EvaluateDup0()
        {
            var name = $"`{Grammer.Dup}` 0";
            var stack = new Stack<Token>();
            var t = new VariableInt(1);
            stack.Push(t);
            WordIncStackEvaluator.Dup(stack);

            if (stack.Count != 2)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"stack count of {stack.Count} not equal to expected count of 2");
                }

                Failures += 1;
            }
            
            var r = stack.Pop();

            if (t.IsEqual(r))
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
                        $"{t.ToStringToken(0)} not equal to {r.ToStringToken(0)}");
                }

                Failures += 1;
            }
        }

        private void EvaluatePop0()
        {
            var name = $"`{Grammer.Pop}` 0";
            var stack = new Stack<Token>();
            var t = new VariableInt(1);
            stack.Push(t);
            WordIncStackEvaluator.Pop(stack);

            if (stack.Count != 0)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"stack count of {stack.Count} not equal to expected count of 0");
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

        private void EvaluateRot0()
        {
            EvaluateRot(new List<Token>
            {
                new VariableInt(0), new VariableInt(1), new VariableInt(2), new VariableInt(3)
            },
            new List<Token>
            {
                new VariableInt(0), new VariableInt(1), new VariableInt(2), new VariableInt(3)
            },
            0,
            "`rot` 0");
        }

        private void EvaluateRot1()
        {
            EvaluateRot(new List<Token>
            {
                new VariableInt(0), new VariableInt(1), new VariableInt(2), new VariableInt(3)
            },
            new List<Token>
            {
                new VariableInt(0), new VariableInt(1), new VariableInt(3), new VariableInt(2)
            },
            1,
            "`rot` 1");
        }


        private void EvaluateRot2()
        {
            EvaluateRot(new List<Token>
            {
                new VariableInt(0), new VariableInt(1), new VariableInt(2), new VariableInt(3)
            },
            new List<Token>
            {
                new VariableInt(0), new VariableInt(2), new VariableInt(3), new VariableInt(1)
            },
            2,
            "`rot` 2");
        }

        #region Helpers

        private void EvaluateUnaryOperation(
            string name, Token t, Token u,
            Action<Stack<Token>, string> eval)
        {
            var stack = new Stack<Token>();
            stack.Push(t);
            eval(stack, name);
            var r = stack.Peek();

            if (u.IsEqual(r))
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
                        $"{u.ToStringToken(0)} not equal to {r.ToStringToken(0)} " +
                        $"for input variable: {t.ToStringToken(0)}");
                }

                Failures += 1;
            }
        }

        private void EvaluateBinaryOperation(
            string name, Token t, Token u, Token v,
            Action<Stack<Token>, string> eval)
        {
            var stack = new Stack<Token>();
            stack.Push(t);
            stack.Push(u);
            eval(stack, name);
            var r = stack.Peek();

            if (v.IsEqual(r))
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
                    PrintFailure($"{name} test failed");
                    PrintFailure($"\t{v.ToStringToken(0)} not equal to {r.ToStringToken(0)}");
                    PrintFailure($"\tfor left input variable: {t.ToStringToken(0)}");
                    PrintFailure($"\tand right input variable: {u.ToStringToken(0)}");
                }

                Failures += 1;
            }
        }

        private void EvaluateRot(List<Token> tokens, List<Token> expected, int rotIndex, string name)
        {
            var stack = new Stack<Token>();
            var expectedStack = new Stack<Token>();

            tokens.ForEach(t => stack.Push(t));
            expected.ForEach(t => expectedStack.Push(t));

            stack.Push(new VariableInt(rotIndex));

            WordIncStackEvaluator.Rot(stack);

            if (stack.Count != expected.Count)
            {
                if (Verbose)
                {
                    PrintFailure($"{name} test failed: " +
                        $"stack count of {stack.Count} not equal to expected count of {expected.Count}");
                }

                Failures += 1;
                return;
            }

            while (stack.Count > 0)
            {
                var r = stack.Pop();
                var e = expectedStack.Pop();

                if (!r.IsEqual(e))
                {
                    if (Verbose)
                    {
                        PrintFailure($"{name} test failed: " +
                            $"{r.ToStringToken(0)} is not equal to expected value of {e.ToStringToken(0)}");
                    }

                    Failures += 1;
                    return;
                }
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
