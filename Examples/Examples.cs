using StackScript0.Core;

namespace StackScript0.Examples
{
    public static class Examples
    {
        public static readonly List<Action> exampleActions = new List<Action>
        {
            () => ViewNestedBranch(),
            () => ViewIterativeFibonacci(),
            () => ViewRecursiveFibonacci()
        };
        
        public static void ViewAll()
        {
            exampleActions.ForEach(e =>
            {
                e();
                Console.WriteLine(Environment.NewLine);
            });
        }

        public static void ViewNestedBranch()
        {
            Console.WriteLine("---- Nested Branch Condition ----");
            ViewIndividual(NestedBranch(2, 3));
        }

        public static void ViewIterativeFibonacci()
        {
            Console.WriteLine("---- Iterative Fibonacci Sequence ----");
            ViewIndividual(IterativeFibonacci(5));
        }

        public static void ViewRecursiveFibonacci()
        {
            Console.WriteLine("---- Recursive Fibonacci Sequence ----");
            ViewIndividual(RecursiveFibonacci(5));
        }

        public static void ViewIndividual(List<Token> tokens)
        {
            tokens.ForEach(t =>
            {
                Console.Write($"{t.ToStringToken(0)} ");
            });
        }

        public static Func<long, long, List<Token>> NestedBranch = (a, b) => new List<Token>
        {
            // base branch
            new Token(TokenType.EXPBEGIN),
            new VariableInt(a),
            new VariableInt(b),
            new Word(Grammer.Equal),
            new Token(TokenType.EXPEND),
            new Token(TokenType.EXPBEGIN),
            new VariableString("Equal!"),
            new Token(TokenType.EXPEND),
            new Token(TokenType.EXPBEGIN),

            // nested branch
            new Token(TokenType.EXPBEGIN),
            new VariableInt(a),
            new VariableInt(b),
            new Word(Grammer.Lower),
            new Token(TokenType.EXPEND),
            new Token(TokenType.EXPBEGIN),
            new VariableString("Lower."),
            new Token(TokenType.EXPEND),
            new Token(TokenType.EXPBEGIN),
            new VariableString("Higher."),
            new Token(TokenType.EXPEND),
            new Token(TokenType.EXPBRANCH),

            new Token(TokenType.EXPEND),
            new Token(TokenType.EXPBRANCH)
        };

        public static Func<long, List<Token>> IterativeFibonacci = index => new List<Token>
        {
            // Fibonnaci index values 0 and 1
            new VariableInt(0),
            new VariableInt(1),
            //
            // while loop values
            new Token(TokenType.WORDBEGIN),
            new Word("i"),
            new VariableInt(0),
            new Token(TokenType.WORDEND),
            new Token(TokenType.WORDBEGIN),
            new Word("index"),
            new VariableInt(index),
            new Token(TokenType.WORDEND),
            //
            // while loop test
            new Token(TokenType.EXPBEGIN),
            new Word("i"),
            new Word("index"),
            new Word(Grammer.Lower),
            new Token(TokenType.EXPEND),
            //
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
            //
            // while loop word
            new Token(TokenType.EXPWHILE)
        };

        public static Func<long, List<Token>> RecursiveFibonacci = index => new List<Token>
        {
            // Fibonacci function
            new Token(TokenType.WORDBEGIN),
            new Word("fib"),
            //
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
            new Token(TokenType.EXPIF),
            new Token(TokenType.WORDEND),
            //
            // starting index values
            new VariableInt(0),
            new Token(TokenType.WORDBEGIN),
            new Word("i"),
            new Token(TokenType.WORDEND),
            new VariableInt(index),
            new Token(TokenType.WORDBEGIN),
            new Word("index"),
            new Token(TokenType.WORDEND),
            //
            // starting Fibonacci values
            new VariableInt(0),
            new VariableInt(1),
            // call function
            new Word("fib")
        };
    }
}
