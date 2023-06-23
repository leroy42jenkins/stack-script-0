using System.Text;

namespace StackScript0.Core
{
    public static class Tokenizer
    {
        private static readonly Dictionary<string, Func<Token>> GrammerTokenMap = new Dictionary<string, Func<Token>>
        {
            { Grammer.WordBegin, () => new Token(TokenType.WORDBEGIN) },
            { Grammer.WordEnd, () => new Token(TokenType.WORDEND) },
            { Grammer.ExpBegin, () => new Token(TokenType.EXPBEGIN) },
            { Grammer.ExpEnd, () => new Token(TokenType.EXPEND) },
            { Grammer.ExpDo, () => new Token(TokenType.EXPDO) },
            { Grammer.ExpIf, () => new Token(TokenType.EXPIF) },
            { Grammer.ExpBranch, () => new Token(TokenType.EXPBRANCH) },
            { Grammer.ExpWhile, () => new Token(TokenType.EXPWHILE) }
        };

        public static List<Token> SegmentsToTokens(List<string> segments) =>
            segments
                .Select(SegmentToToken)
                .ToList();

        public static Token SegmentToToken(string segment)
        {
            if (GrammerTokenMap.ContainsKey(segment))
                return GrammerTokenMap[segment]();
            
            if (Grammer.IsValidInt(segment) && long.TryParse(segment, out long l))
                return new VariableInt(l);

            if (double.TryParse(segment, out double d))
                return new VariableFloat(d);

            if (Grammer.IsValidStringLiteral(segment))
                return new VariableString(Grammer.StringLiteralToString(segment));

            return new Word(segment);
        }

        public static List<string> SourceToSegments(string source)
        {
            var segments = new List<string>();
            var currentSegment = new StringBuilder();
            var insideString = false;
            var insideComment = false;

            for (var i = 0; i < source.Length; ++i)
            {
                var current = source[i];

                if (insideString)
                {
                    currentSegment.Append(current);

                    if (Grammer.IsStringLiteralStop(source, i))
                        insideString = false;
                }
                else if (insideComment)
                {
                    if (Grammer.IsCommentStop(source, i))
                        insideComment = false;
                }
                else
                {
                    if (Grammer.IsStringLiteralStart(current))
                    {
                        if (currentSegment.Length > 0)
                            segments.Add(currentSegment.ToString());

                        currentSegment = new StringBuilder();
                        currentSegment.Append(current);
                        insideString = true;
                    }
                    else if (Grammer.IsCommentStart(current))
                    {
                        if (currentSegment.Length > 0)
                            segments.Add(currentSegment.ToString());

                        currentSegment = new StringBuilder();
                        insideComment = true;
                    }
                    else if (Grammer.IsBracket(current))
                    {
                        if (currentSegment.Length > 0)
                            segments.Add(currentSegment.ToString());

                        segments.Add(current.ToString());

                        currentSegment = new StringBuilder();
                    }
                    else if (Grammer.IsWhiteSpace(current))
                    {
                        if (currentSegment.Length > 0)
                            segments.Add(currentSegment.ToString());

                        currentSegment = new StringBuilder();
                    }
                    else if (!Grammer.IsCharToSkip(current))
                    {
                        currentSegment.Append(current);
                    }
                }
            }

            if (insideString)
                throw new Exception("incomplete string in source sequence");

            if (insideComment)
                throw new Exception("incomplete comment in source sequence");

            if (currentSegment.Length > 0)
                segments.Add(currentSegment.ToString());

            return segments;
        }
    }
}
