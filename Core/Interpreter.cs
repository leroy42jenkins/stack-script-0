namespace StackScript0.Core
{
    public class Interpreter
    {
        private Dictionary<string, Word> Definitions { get; set; }
        private Stack<Token> TokenStack { get; set; }

        public Interpreter()
        {
            Definitions = new Dictionary<string, Word>();
            TokenStack = new Stack<Token>();
        }

        public void Eval(string source)
        {
            var segments = Tokenizer.SourceToSegments(source);
            var tokens = Tokenizer.SegmentsToTokens(segments);

            GeneralEvaluator.Eval(Definitions, TokenStack, tokens);
        }

        public void REPL()
        {
            string? s;
            var interpreter = new Interpreter();

            while (true)
            {
                try
                {
                    s = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(s))
                        Eval(s);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
