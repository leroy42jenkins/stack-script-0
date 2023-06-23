namespace StackScript0.Core
{
    public static class GeneralEvaluator
    {
        private static readonly Dictionary<TokenType, Action<Token, Stack<Token>>> EvalVarActionMap =
            new Dictionary<TokenType, Action<Token, Stack<Token>>>
            {
                { TokenType.INT, (n, tokenStack) => tokenStack.Push(n) },
                { TokenType.FLOAT, (n, tokenStack) => tokenStack.Push(n) },
                { TokenType.STRING, (n, tokenStack) => tokenStack.Push(n) },
                { TokenType.TABLE, (n, tokenStack) => tokenStack.Push(n) }
            };

        private static readonly Dictionary<TokenType, Action<Dictionary<string, Word>, Stack<Token>>> EvalControlActionMap =
            new Dictionary<TokenType, Action<Dictionary<string, Word>, Stack<Token>>>
            {
                { TokenType.EXPDO, (definitions, tokenStack) => EvalDo(definitions, tokenStack) },
                { TokenType.EXPIF, (definitions, tokenStack) => EvalIf(definitions, tokenStack) },
                { TokenType.EXPBRANCH, (definitions, tokenStack) => EvalBranch(definitions, tokenStack) },
                { TokenType.EXPWHILE, (definitions, tokenStack) => EvalWhile(definitions, tokenStack) }
            };


        private static readonly Dictionary<string, Action<Stack<Token>>> WordIncStackActionMap =
            new Dictionary<string, Action<Stack<Token>>>
            {
                { Grammer.Add, stack => WordIncStackEvaluator.Add(stack, Grammer.Add) },
                { Grammer.Subtract, stack => WordIncStackEvaluator.Subtract(stack, Grammer.Subtract) },
                { Grammer.Multiply, stack => WordIncStackEvaluator.Multiply(stack, Grammer.Multiply) },
                { Grammer.Divide, stack => WordIncStackEvaluator.Divide(stack, Grammer.Divide) },
                { Grammer.Modulus, stack => WordIncStackEvaluator.Modulus(stack, Grammer.Modulus) },
                { Grammer.Lower, stack => WordIncStackEvaluator.Lower(stack, Grammer.Lower) },
                { Grammer.Equal, stack => WordIncStackEvaluator.Equal(stack, Grammer.Equal) },
                { Grammer.Higher, stack => WordIncStackEvaluator.Higher(stack, Grammer.Higher) },
                { Grammer.Not, stack => WordIncStackEvaluator.Not(stack, Grammer.Not) },
                { Grammer.Or, stack => WordIncStackEvaluator.Or(stack, Grammer.Or) },
                { Grammer.And, stack => WordIncStackEvaluator.And(stack, Grammer.And) },
                { Grammer.CastInt, stack => WordIncStackEvaluator.CastInt(stack, Grammer.CastInt) },
                { Grammer.CastFloat, stack => WordIncStackEvaluator.CastFloat(stack, Grammer.CastFloat) },
                { Grammer.CastString, stack => WordIncStackEvaluator.CastString(stack, Grammer.CastString) },
                { Grammer.Type, stack => WordIncStackEvaluator.Type(stack, Grammer.Type) },
                { Grammer.Dup, stack => WordIncStackEvaluator.Dup(stack) },
                { Grammer.Pop, stack => WordIncStackEvaluator.Pop(stack) },
                { Grammer.Rot, stack => WordIncStackEvaluator.Rot(stack) },
                { Grammer.TokenSet, stack => EvalSet(stack) },
                { Grammer.TokenGet, stack => EvalGet(stack) },
                { Grammer.TokenHas, stack => EvalHas(stack) },
                { Grammer.TokenTable, stack => EvalTable(stack) },
                { Grammer.ViewStack, stack => ViewStack(stack) },
                { Grammer.ClearStack, stack => ClearStack(stack) }
            };

        private static readonly Dictionary<string, Action<Dictionary<string, Word>>> WordIncDefActionMap =
            new Dictionary<string, Action<Dictionary<string, Word>>>
            {
                { Grammer.ViewDefinitions, definitions => ViewDefinitions(definitions) },
                { Grammer.ClearDefinitions, definitions => ClearDefinitions(definitions) }
            };

        public static void Eval(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack,
            List<Token> tokens)
        {
            var index = 0;

            while (index < tokens.Count)
            {
                var next = tokens[index];

                if (EvalVarActionMap.ContainsKey(next.Type))
                {
                    EvalVarActionMap[next.Type](next, tokenStack);
                    index += 1;
                }
                else if (EvalControlActionMap.ContainsKey(next.Type))
                {
                    EvalControlActionMap[next.Type](definitions, tokenStack);
                    index += 1;
                }
                else if (next.Type == TokenType.WORD)
                {
                    var w = (Word)next;
                    EvalWord(definitions, tokenStack, w.Name);
                    index += 1;
                }
                else if (next.Type == TokenType.WORDBEGIN)
                {
                    index = DefineWord(definitions, tokenStack, tokens, index);
                }
                else if (next.Type == TokenType.EXPBEGIN)
                {
                    var tuple = FormExpressionFromTokenSequence(tokens, index);
                    var exp = tuple.Item1;
                    tokenStack.Push(exp);
                    index = tuple.Item2;
                }
                else
                {
                    throw new Exception($"token of type: {next.Type} in sequence is illogical");
                }
            }
        }

        public static void EvalWord(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack,
            string name)
        {
            if (WordIncStackActionMap.ContainsKey(name))
                WordIncStackActionMap[name](tokenStack);
            else if (WordIncDefActionMap.ContainsKey(name))
                WordIncDefActionMap[name](definitions);
            else if (name == Grammer.ConsoleIn)
                EvalConsoleIn(definitions, tokenStack);
            else if (name == Grammer.ConsoleOut)
                EvalConsoleOut(tokenStack);
            else if (name == Grammer.FileIn)
                EvalFileIn(tokenStack);
            else if (name == Grammer.FileOut)
                EvalFileOut(tokenStack);
            else if (name == Grammer.FileRem)
                EvalFileRem(tokenStack);
            else
                EvalWordFromUser(definitions, tokenStack, name);
        }

        public static void EvalWordFromUser(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack,
            string name)
        {
            if (definitions.ContainsKey(name))
            {
                var word = definitions[name];
                Eval(definitions, tokenStack, word.Tokens);
            }
            else
            {
                throw new Exception($"word: {name} is undefined");
            }
        }

        public static void EvalDo(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 1, Grammer.ExpDo);
            var d = tokenStack.Pop();

            if (d.Type == TokenType.EXP)
            {
                var exp = (Expression)d;
                Eval(definitions, tokenStack, exp.Tokens);
            }
            else if (d.Type == TokenType.STRING)
            {
                var text = ((VariableString)d).Value;
                var unescaped = Grammer.EscapedStringToString(text);
                var segments = Tokenizer.SourceToSegments(unescaped);
                var tokens = Tokenizer.SegmentsToTokens(segments);
                Eval(definitions, tokenStack, tokens);
            }
            else
            {
                throw new Exception($"invalid type for {Grammer.ExpDo}, " +
                    $"must be {TokenType.EXP} or {TokenType.STRING}");
            }
        }

        public static void EvalIf(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 2, Grammer.ExpIf);
            var b = tokenStack.Pop();
            var t = tokenStack.Pop();
            TestExpType(b, Grammer.ExpIf);
            TestExpType(t, Grammer.ExpIf);
            var body = (Expression)b;
            var test = (Expression)t;
            Eval(definitions, tokenStack, test.Tokens);
            var top = tokenStack.Pop();

            if (VarOpEvaluator.IsTrue(top))
                Eval(definitions, tokenStack, body.Tokens);
        }

        public static void EvalBranch(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 3, Grammer.ExpBranch);
            var f = tokenStack.Pop();
            var p = tokenStack.Pop();
            var t = tokenStack.Pop();
            TestExpType(f, Grammer.ExpBranch);
            TestExpType(p, Grammer.ExpBranch);
            TestExpType(t, Grammer.ExpBranch);
            var fail = (Expression)f;
            var pass = (Expression)p;
            var test = (Expression)t;
            Eval(definitions, tokenStack, test.Tokens);
            var top = tokenStack.Pop();

            if (VarOpEvaluator.IsTrue(top))
                Eval(definitions, tokenStack, pass.Tokens);
            else
                Eval(definitions, tokenStack, fail.Tokens);
        }

        public static void EvalWhile(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 2, Grammer.ExpWhile);
            var b = tokenStack.Pop();
            var t = tokenStack.Pop();
            TestExpType(b, Grammer.ExpWhile);
            TestExpType(t, Grammer.ExpWhile);
            var body = (Expression)b;
            var test = (Expression)t;

            while (true)
            {
                Eval(definitions, tokenStack, test.Tokens);
                var top = tokenStack.Pop();

                if (VarOpEvaluator.IsTrue(top))
                    Eval(definitions, tokenStack, body.Tokens);
                else
                    break;
            }
        }

        public static void EvalSet(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 3, Grammer.TokenSet);
            var v = tokenStack.Pop();
            var k = tokenStack.Pop();
            var t = tokenStack.Pop();
            TestTokenType(k, TokenType.STRING, Grammer.TokenSet);
            TestTokenType(t, TokenType.TABLE, Grammer.TokenSet);
            var key = ((VariableString)k).Value;
            var table = (VariableTable)t;

            if (table.Value.ContainsKey(key))
                table.Value[key] = v;
            else
                table.Value.Add(key, v);

            tokenStack.Push(table);
        }

        public static void EvalGet(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 2, Grammer.TokenGet);
            var k = tokenStack.Pop();
            var t = tokenStack.Pop();
            TestTokenType(k, TokenType.STRING, Grammer.TokenGet);
            TestTokenType(t, TokenType.TABLE, Grammer.TokenGet);
            var key = ((VariableString)k).Value;
            var table = (VariableTable)t;

            var token = table.Value.ContainsKey(key)
                ? table.Value[key]
                : new Token();

            tokenStack.Push(table);
            tokenStack.Push(token);
        }

        public static void EvalHas(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 2, Grammer.TokenHas);
            var k = tokenStack.Pop();
            var t = tokenStack.Pop();
            TestTokenType(k, TokenType.STRING, Grammer.TokenHas);
            TestTokenType(t, TokenType.TABLE, Grammer.TokenHas);
            var key = ((VariableString)k).Value;
            var table = (VariableTable)t;

            var token = table.Value.ContainsKey(key)
                ? new VariableInt(1)
                : new VariableInt(0);

            tokenStack.Push(table);
            tokenStack.Push(token);
        }

        public static void EvalTable(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 1, Grammer.TokenTable);
            var t = tokenStack.Pop();
            TestExpType(t, Grammer.TokenTable);
            var exp = (Expression)t;

            var tableDefinitions = new Dictionary<string, Token>();
            var key = "";
            var counter = 0;
            var index = 0;

            while (index < exp.Tokens.Count)
            {
                var next = exp.Tokens[index];

                if (counter % 2 == 0)
                {
                    if (next.Type == TokenType.STRING)
                    {
                        key = ((VariableString)next).Value;
                        index += 1;
                    }
                    else
                    {
                        throw new Exception($"invalid key type for table, must be string");
                    }
                }
                else
                {
                    if (next.Type == TokenType.EXPBEGIN)
                    {
                        var tuple = FormExpressionFromTokenSequence(exp.Tokens, index);
                        var e = tuple.Item1;
                        tableDefinitions.Add(key, e);
                        index = tuple.Item2;
                    }
                    else
                    {
                        tableDefinitions.Add(key, next);
                        index += 1;
                    }
                }

                counter += 1;
            }

            tokenStack.Push(new VariableTable(tableDefinitions));
        }

        public static void EvalConsoleIn(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack)
        {
            var source = Console.ReadLine();

            if (source == null)
                throw new Exception("console input cannot be null");

            var segments = Tokenizer.SourceToSegments(source);
            var tokens = Tokenizer.SegmentsToTokens(segments);

            Eval(definitions, tokenStack, tokens);
        }

        public static void EvalConsoleOut(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 1, Grammer.ConsoleOut);
            var t = tokenStack.Peek();
            Console.WriteLine(t.ToStringToken(0));
        }

        public static void EvalFileIn(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 1, Grammer.FileIn);
            var fileName = ((VariableString)tokenStack.Pop()).Value;
            var text = File.ReadAllText(fileName);
            var escaped = Grammer.StringToEscapedString(text);
            tokenStack.Push(new VariableString(escaped));
        }

        public static void EvalFileOut(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 2, Grammer.FileOut);
            var fileName = ((VariableString)tokenStack.Pop()).Value;
            var text = ((VariableString)tokenStack.Peek()).Value;
            var unescaped = Grammer.EscapedStringToString(text);
            File.WriteAllText(fileName, unescaped);
        }

        public static void EvalFileRem(Stack<Token> tokenStack)
        {
            TestStackCount(tokenStack, 1, Grammer.FileRem);
            var fileName = ((VariableString)tokenStack.Pop()).Value;
            File.Delete(fileName);
        }

        public static void ViewStack(Stack<Token> tokenStack)
        {
            tokenStack.ToList().ForEach(token =>
            {
                Console.WriteLine(token.ToStringToken(0));
            });
        }

        public static void ClearStack(Stack<Token> tokenStack)
        {
            tokenStack.Clear();
        }

        public static void ViewDefinitions(Dictionary<string, Word> definitions)
        {
            definitions.ToList().ForEach(pair =>
            {
                var k = pair.Key;
                var v = pair.Value;

                var tokenStrings = v.Tokens
                    .Select(t => t.Type == TokenType.WORD
                        ? ((Word)t).Name
                        : t.ToStringToken(0));

                Console.WriteLine($"{k} : {string.Join(' ', tokenStrings)}");
            });
        }

        public static void ClearDefinitions(Dictionary<string, Word> definitions)
        {
            definitions.Clear();
        }

        // pass in index of word definition beginning
        public static int DefineWord(
            Dictionary<string, Word> definitions,
            Stack<Token> tokenStack,
            List<Token> tokens,
            int i)
        {
            if (i + 2 >= tokens.Count)
                throw new Exception("insufficient token number to define word");

            // move to next token after beginning
            i += 1;

            if (tokens[i].Type != TokenType.WORD)
                throw new Exception($"invalid token type of {tokens[i].Type} for word name");

            var wordName = ((Word)tokens[i]).Name;

            if (Grammer.IsReserved(wordName))
                throw new Exception($"{wordName} is a reserved word name");

            var wordTokens = new List<Token>();

            // move to next token after name
            i += 1;

            if (tokens[i].Type == TokenType.WORDEND)
                // lift token from stack as word token
                i = ConfigureWordFromTokenStack(tokenStack, wordTokens, i);
            else
                // define word tokens from sequence which follows
                i = ConfigureWordFromTokenSequence(tokens, wordTokens, i);

            AddWordToDefinitions(definitions, wordTokens, wordName);

            return i;
        }

        // pass in index of expression beginning
        public static Tuple<Token, int> FormExpressionFromTokenSequence(
            List<Token> tokens,
            int i)
        {
            if (i + 1 >= tokens.Count)
                throw new Exception("insufficient token number to push expression onto stack");

            var expTokens = new List<Token>();

            // move to next token after beginning
            i += 1;
            var depth = 0;

            while (i < tokens.Count)
            {
                var t = tokens[i];

                if (t.Type == TokenType.EXPBEGIN)
                {
                    expTokens.Add(t.GetClone());
                    depth += 1;
                    i += 1;
                }
                else if (t.Type == TokenType.EXPEND)
                {
                    if (depth == 0)
                    {
                        i += 1;
                        break;
                    }
                    else
                    {
                        expTokens.Add(t.GetClone());
                        depth -= 1;

                        if (depth < 0)
                            throw new Exception("imbalanced expression");

                        i += 1;
                    }
                }
                else
                {
                    expTokens.Add(t.GetClone());
                    i += 1;
                }
            }

            if (depth != 0)
                throw new Exception("imbalanced expression");

            return new Tuple<Token, int>(new Expression(expTokens), i);
        }

        private static int ConfigureWordFromTokenStack(
            Stack<Token> tokenStack,
            List<Token> wordTokens,
            int i)
        {
            TestStackCount(tokenStack, 1, TokenType.WORD.ToString());

            wordTokens.Add(tokenStack.Pop());

            return i + 1;
        }

        private static int ConfigureWordFromTokenSequence(
            List<Token> tokens,
            List<Token> wordTokens,
            int i)
        {
            var depth = 0;

            while (i < tokens.Count)
            {
                var t = tokens[i];

                if (t.Type == TokenType.WORDBEGIN)
                {
                    wordTokens.Add(t.GetClone());
                    depth += 1;
                    i += 1;
                }
                else if (t.Type == TokenType.WORDEND)
                {
                    if (depth == 0)
                    {
                        i += 1;
                        break;
                    }
                    else
                    {
                        wordTokens.Add(t.GetClone());
                        depth -= 1;
                        i += 1;
                    }
                }
                else
                {
                    wordTokens.Add(t.GetClone());
                    i += 1;
                }
            }

            if (depth != 0)
                throw new Exception("imbalanced word definition");

            return i;
        }

        private static void AddWordToDefinitions(
            Dictionary<string, Word> definitions,
            List<Token> tokens,
            string name)
        {
            var word = new Word(name, tokens);

            if (definitions.ContainsKey(name))
            {
                if (tokens.Count == 1)
                {
                    var t = tokens.First();
                    
                    if (t.ToStringToken(0) == Grammer.ClearDefinition)
                    {
                        definitions.Remove(name);
                        return;
                    }
                }

                definitions[name] = word;
            }
            else
            {
                definitions.Add(name, word);
            }
        }

        private static void TestStackCount(
            Stack<Token> tokenStack,
            int expected,
            string type)
        {
            if (tokenStack.Count < expected)
                throw new Exception($"insufficient token number of {tokenStack.Count} for {type} evaluation");
        }

        private static void TestExpType(Token t, string typeName)
        {
            TestTokenType(t, TokenType.EXP, typeName);
        }

        private static void TestTokenType(Token t, TokenType type, string typeName)
        {
            if (t.Type != type)
                throw new Exception($"invalid type for {typeName}, must be {type}");
        }
    }
}
