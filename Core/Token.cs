namespace StackScript0.Core
{
    public enum TokenType
    {
        NONE,
        INT,
        FLOAT,
        STRING,
        TABLE,
        WORDBEGIN,
        WORDEND,
        WORD,
        EXPBEGIN,
        EXPEND,
        EXP,
        EXPDO,
        EXPIF,
        EXPBRANCH,
        EXPWHILE
    }

    public static class TokenHelpers
    {
        private static readonly Dictionary<TokenType, string> TokenTypeMap = new Dictionary<TokenType, string>
        {
            { TokenType.EXPBEGIN, Grammer.ExpBegin },
            { TokenType.EXPEND, Grammer.ExpEnd },
            { TokenType.EXPDO, Grammer.ExpDo },
            { TokenType.EXPIF, Grammer.ExpIf },
            { TokenType.EXPBRANCH, Grammer.ExpBranch },
            { TokenType.EXPWHILE, Grammer.ExpWhile },
            { TokenType.WORDBEGIN, Grammer.WordBegin },
            { TokenType.WORDEND, Grammer.WordEnd }
        };

        public static string TokenTypeToString(TokenType type) =>
            TokenTypeMap.ContainsKey(type)
                ? TokenTypeMap[type]
                : type.ToString();

        public static string StringToStringLiteral(string s) =>
            $"{Grammer.StringIdentifier}{s}{Grammer.StringIdentifier}";
    }

    public class Token
    {
        public TokenType Type { protected set; get; }

        public Token()
        {
            Type = TokenType.NONE;
        }

        public Token(TokenType type)
        {
            Type = type;
        }

        protected string GetSpaces(int depth)
        {
            string spaces = "";

            for (int i = 0; i < depth; ++i)
                spaces += " ";

            return spaces;
        }

        public virtual string ToStringToken(int depth) =>
            GetSpaces(depth) + TokenHelpers.TokenTypeToString(Type);

        public virtual Token GetClone() => new Token(Type);

        public virtual bool IsEqual(Token v) => Type == v.Type;
    }

    #region Basic Variables: { None, Int, Float, String, Table }

    public class VariableNone : Token
    {
        public VariableNone()
        {
            Type = TokenType.NONE;
        }

        public override Token GetClone() => new VariableNone();
    }

    public class VariableInt : Token
    {
        public long Value { set; get; }

        public VariableInt(long value)
        {
            Type = TokenType.INT;
            Value = value;
        }

        public override Token GetClone() =>
            new VariableInt(Value);

        public override string ToStringToken(int depth) =>
            GetSpaces(depth) + Value.ToString();

        public override bool IsEqual(Token v) =>
            base.IsEqual(v) && Value == ((VariableInt)v).Value;
    }

    public class VariableFloat : Token
    {
        public double Value { set; get; }

        public VariableFloat(double value)
        {
            Type = TokenType.FLOAT;
            Value = value;
        }

        public override Token GetClone() =>
            new VariableFloat(Value);

        public override string ToStringToken(int depth) =>
            GetSpaces(depth) + Value.ToString();

        public override bool IsEqual(Token v) =>
            base.IsEqual(v) && Value == ((VariableFloat)v).Value;
    }

    public class VariableString : Token
    {
        public string Value { set; get; }

        public VariableString(string value)
        {
            Type = TokenType.STRING;
            Value = value;
        }

        public override Token GetClone() =>
            new VariableString(Value);

        public override string ToStringToken(int depth) =>
            GetSpaces(depth) + TokenHelpers.StringToStringLiteral(Value);

        public override bool IsEqual(Token v) =>
            base.IsEqual(v) && Value == ((VariableString)v).Value;
    }

    public class VariableTable : Token
    {
        public Dictionary<string, Token> Value { set; get; }

        public VariableTable(Dictionary<string, Token> value)
        {
            Type = TokenType.TABLE;
            Value = value;
        }

        public override Token GetClone()
        {
            var dict = new Dictionary<string, Token>();

            foreach (var pair in Value)
                dict.Add(pair.Key, pair.Value.GetClone());

            return new VariableTable(dict);
        }

        public override string ToStringToken(int depth)
        {
            var lines = new List<string>();
            var spaces = GetSpaces(depth);

            foreach (var pair in Value)
            {
                var k = pair.Key;
                var v = pair.Value;

                if (v.Type == TokenType.TABLE)
                {
                    lines.Add($"{spaces}{TokenHelpers.StringToStringLiteral(k)} :");
                    lines.Add($"{v.ToStringToken(depth + 2)}");
                }
                else
                {
                    lines.Add($"{spaces}" +
                        $"{TokenHelpers.StringToStringLiteral(k)} : " +
                        $"{v.ToStringToken(0)}");
                }
            }

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        public override bool IsEqual(Token t)
        {
            if (!base.IsEqual(t))
                return false;

            var table = (VariableTable)t;

            if (Value.Count != table.Value.Count)
                return false;

            foreach (var pair in Value)
            {
                var k = pair.Key;
                var v = pair.Value;

                if (!table.Value.ContainsKey(k))
                    return false;

                if (!table.Value[k].IsEqual(v))
                    return false;
            }

            return true;
        }
    }

    #endregion Basic Variables: { None, Int, Float, String, Table }

    #region Expression

    public class Expression : Token
    {
        public List<Token> Tokens { private set; get; }

        public Expression()
        {
            Type = TokenType.EXP;
            Tokens = new List<Token>();
        }

        public Expression(List<Token> tokens)
        {
            Type = TokenType.EXP;
            Tokens = tokens;
        }

        public override Token GetClone() =>
            new Expression(
                Tokens
                    .Select(v => v.GetClone())
                    .ToList());

        public override bool IsEqual(Token v)
        {
            if (!base.IsEqual(v)) return false;

            var extExp = (Expression)v;

            if (Tokens.Count != extExp.Tokens.Count) return false;

            for (var i = 0; i < Tokens.Count; ++i)
                if (!Tokens[i].IsEqual(extExp.Tokens[i])) return false;

            return true;
        }

        public override string ToStringToken(int depth)
        {
            var lines = new List<string> { $"{Grammer.ExpBegin}" };

            Tokens.ForEach(v =>
            {
                lines.Add(v.ToStringToken(2));
            });

            lines.Add(Grammer.ExpEnd);

            return string.Join(Environment.NewLine, lines);
        }
    }

    #endregion Expression

    #region Word

    public class Word : Token
    {
        public string Name { private set; get; }
        public List<Token> Tokens { private set; get; }

        public Word(string name)
        {
            Type = TokenType.WORD;
            Name = name;
            Tokens = new List<Token>();
        }

        public Word(string name, List<Token> tokens)
        {
            Type = TokenType.WORD;
            Name = name;
            Tokens = tokens;
        }

        public override Token GetClone() =>
            new Word(
                Name,
                Tokens
                    .Select(v => v.GetClone())
                    .ToList());

        public override bool IsEqual(Token v)
        {
            if (!base.IsEqual(v)) return false;

            var extWord = (Word)v;

            if (Tokens.Count != extWord.Tokens.Count) return false;

            for (var i = 0; i < Tokens.Count; ++i)
                if (!Tokens[i].IsEqual(extWord.Tokens[i])) return false;

            return true;
        }

        public override string ToStringToken(int depth) =>
            Name;
    }

    #endregion Word
}
