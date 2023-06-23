namespace StackScript0.Core
{
    public static class WordIncStackEvaluator
    {
        public static void Not(Stack<Token> stack, string name) =>
            UnaryOp(stack, VarOpEvaluator.Not, name);

        public static void CastInt(Stack<Token> stack, string name) =>
            UnaryOp(stack, VarOpEvaluator.ToInt, name);

        public static void CastFloat(Stack<Token> stack, string name) =>
            UnaryOp(stack, VarOpEvaluator.ToFloat, name);

        public static void CastString(Stack<Token> stack, string name) =>
            UnaryOp(stack, VarOpEvaluator.ToString, name);

        public static void Type(Stack<Token> stack, string name) =>
            UnaryOp(stack, VarOpEvaluator.ToTypeString, name);

        public static void Or(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Or, name);

        public static void And(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.And, name);

        public static void Lower(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Lower, name);

        public static void Equal(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Equal, name);

        public static void Higher(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Higher, name);

        public static void Add(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Add, name);

        public static void Subtract(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Subtract, name);

        public static void Multiply(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Multiply, name);

        public static void Divide(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Divide, name);

        public static void Modulus(Stack<Token> stack, string name) =>
            BinaryOp(stack, VarOpEvaluator.Modulus, name);

        public static void Dup(Stack<Token> stack)
        {
            if (stack.Count < 1)
                throw new Exception("stack must have at least one token to duplicate");

            var t = stack.Peek();
            stack.Push(t.GetClone());
        }

        public static void Pop(Stack<Token> stack)
        {
            if (stack.Count < 1)
                throw new Exception("stack must have at least one token to pop");

            stack.Pop();
        }

        public static void Rot(Stack<Token> stack)
        {
            if (stack.Count < 2)
                throw new Exception("stack must have at least one integer and one other token to rotate");

            var t = stack.Pop();

            if (t.Type != TokenType.INT)
                throw new Exception("first token must be an integer");

            var v = ((VariableInt)t).Value;
            var temp = new Stack<Token>();
            var top = new Token(TokenType.NONE);

            for (var j = 0; stack.Count > 0 && j <= v; ++j)
            {
                if (j < v)
                    temp.Push(stack.Pop());
                else
                    top = stack.Pop();
            }

            while (temp.Count > 0)
                stack.Push(temp.Pop());

            if (top.Type != TokenType.NONE)
                stack.Push(top);
        }

        private static void UnaryOp(Stack<Token> stack, Func<Token, Token> op, string name)
        {
            if (stack.Count < 1)
                throw new Exception($"unary operation `{name}` requires two token from stack");

            var t = stack.Pop();
            var v = op(t);

            if (v.Type == TokenType.NONE)
                throw new Exception($"unsupported token for `{name}` operation");

            stack.Push(v);
        }


        private static void BinaryOp(Stack<Token> stack, Func<Token, Token, Token> op, string name)
        {
            if (stack.Count < 2)
                throw new Exception($"binary operation `{name}` requires two tokens from stack");

            var t = stack.Pop();
            var u = stack.Pop();
            var v = op(u, t);

            if (v.Type == TokenType.NONE)
                throw new Exception($"unsupported tokens for `{name}` operation");

            stack.Push(v);
        }
    }
}
