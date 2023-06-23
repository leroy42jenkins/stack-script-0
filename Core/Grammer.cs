using System.Text.RegularExpressions;

namespace StackScript0.Core
{
    public static class Grammer
    {
        public const char CommentIdentifier = '#';
        public const char StringIdentifier = '`';

        public const string Comment = "#";

        public const string Add = "+";
        public const string Subtract = "-";
        public const string Multiply = "*";
        public const string Divide = "/";
        public const string Modulus = "mod";

        public const string Lower = "lo";
        public const string Equal = "eq";
        public const string Higher = "hi";

        public const string Not = "not";
        public const string Or = "or";
        public const string And = "and";

        public const string CastInt = "int";
        public const string CastFloat = "float";
        public const string CastString = "string";

        public const string Type = "type";

        public const string Dup = "dup";
        public const string Pop = "pop";
        public const string Rot = "rot";

        public const string WordBegin = "{";
        public const string WordEnd = "}";

        public const string ExpBegin = "[";
        public const string ExpEnd = "]";

        public const string ExpDo = "do";
        public const string ExpIf = "if";
        public const string ExpBranch = "branch";
        public const string ExpWhile = "while";

        public const string TokenSet = "set";
        public const string TokenGet = "get";
        public const string TokenHas = "has";

        public const string TokenTable = "table";

        public const string ConsoleIn = "cin";
        public const string ConsoleOut = "cout";

        public const string FileIn = "fin";
        public const string FileOut = "fout";
        public const string FileRem = "frem";

        public const string ViewStack = ".";
        public const string ClearStack = "cl";
        public const string ViewDefinitions = "defs";
        public const string ClearDefinition = "--";
        public const string ClearDefinitions = "cl-defs";

        public static readonly Dictionary<string, string> Descriptions = new Dictionary<string, string>
        {
            { Comment, "denotes a comment" },
            { Add, "performs addition" },
            { Subtract, "performs subtration" },
            { Multiply, "performs multiplication" },
            { Divide, "performs division" },
            { Modulus, "determines remainder of division" },
            { Lower, "determines if first token is lower than second token" },
            { Equal, "determines if first token is equal to second token" },
            { Higher, "determines if first token is higher than second token" },
            { Not, "performs negation" },
            { Or, $"performs logical {StringIdentifier}or{StringIdentifier} operation" },
            { And, $"performs logical {StringIdentifier}and{StringIdentifier} operation" },
            { CastInt, "cast to int" },
            { CastFloat, "cast to float" },
            { CastString, "cast to string" },
            { Type, "get string representation of type" },
            { Dup, "duplicate top token of stack" },
            { Pop, "remove top token of stack" },
            { Rot, "rotate tokens to certain depth of stack" },
            { WordBegin, $"begin {StringIdentifier}word{StringIdentifier} definition" },
            { WordEnd, $"end {StringIdentifier}word{StringIdentifier} definition" },
            { ExpBegin, "begin expression" },
            { ExpEnd, "end expression" },
            { ExpDo, "perform expression" },
            { ExpIf, $"perform {StringIdentifier}if{StringIdentifier} condition" +
                $"{Environment.NewLine}\texample: {ExpBegin}test{ExpEnd}{ExpBegin}body-true{ExpEnd} {ExpIf}" },
            { ExpBranch, $"perform {StringIdentifier}branch{StringIdentifier} condition" +
                $"{Environment.NewLine}\texample: {ExpBegin}test{ExpEnd}{ExpBegin}body-true{ExpEnd}{ExpBegin}body-false{ExpEnd} {ExpBranch}" },
            { ExpWhile, $"perform {StringIdentifier}while{StringIdentifier} loop" +
                $"{Environment.NewLine}\texample: {ExpBegin}test{ExpEnd}{ExpBegin}body-true{ExpEnd} {ExpWhile}" },
            { TokenSet, "set token in table" +
                $"{Environment.NewLine}\texample: table {StringIdentifier}key{StringIdentifier} {StringIdentifier}value{StringIdentifier} {TokenSet}" },
            { TokenGet, "get token from table" +
                $"{Environment.NewLine}\texample: table {StringIdentifier}key{StringIdentifier} {TokenGet}" },
            { TokenHas, "test if token exists in table" +
                $"{Environment.NewLine}\texample: table {StringIdentifier}key{StringIdentifier} {TokenHas}" },
            { TokenTable, "create table" +
                $"{Environment.NewLine}\texample: {ExpBegin}{StringIdentifier}key{StringIdentifier} {StringIdentifier}value{StringIdentifier}{ExpEnd} {TokenTable}" },
            { ConsoleIn, "read string from console" },
            { ConsoleOut, "write string to console" },
            { FileIn, "read file into string" +
                $"{Environment.NewLine}\texample: {StringIdentifier}file-name{StringIdentifier} {FileIn}" },
            { FileOut, "write string into file" +
                $"{Environment.NewLine}\texample: {StringIdentifier}string{StringIdentifier} {StringIdentifier}file-name{StringIdentifier} {FileOut}" },
            { FileRem, "remove file" +
                $"{Environment.NewLine}\texample: {StringIdentifier}file-name{StringIdentifier} {FileRem}" },
            { ViewStack, "view contents of stack" },
            { ClearStack, "clear stack" },
            { ViewDefinitions, "view defintions" },
            { ClearDefinition, "clear individual definition" +
                $"{Environment.NewLine}\texample: {WordBegin}word {ClearDefinition}{WordEnd}" },
            { ClearDefinitions, "clear definitions" }
        };

        public static readonly Regex IntRegex = new Regex(@"^-{0,1}[0-9]+$");

        public static bool IsReserved(string s) => Descriptions.ContainsKey(s);

        public static bool IsStringLiteralStart(char c) => c == StringIdentifier;

        public static bool IsStringLiteralStop(string s, int i) => IsUnescapedStop(s, i, StringIdentifier);

        public static bool IsValidStringLiteral(string s) =>
            s.Length >= 2 && IsStringLiteralStart(s[0]) && IsStringLiteralStop(s, s.Length - 1);

        public static string StringLiteralToString(string l) =>
            l.Length <= 2
                ? ""
                : l.Substring(1, l.Length - 2);

        public static string StringToEscapedString(string s)
        {
            var chars = new List<char>();

            for (var i = 0; i < s.Length; ++i)
            {
                if (s[i] == StringIdentifier)
                {
                    chars.Add('\\');
                    chars.Add(s[i]);
                }
                else
                {
                    chars.Add(s[i]);
                }
            }

            return new string(chars.ToArray());
        }

        // assumes escaped format for string
        // a b + \`string\`
        public static string EscapedStringToString(string s)
        {
            var chars = new List<char>();
            var i = 0;
            var j = s.Length - 1;

            while (i < j)
            {
                if (s[i + 1] == StringIdentifier)
                {
                    chars.Add(s[i + 1]);
                    i += 2;
                }
                else
                {
                    chars.Add(s[i]);
                    i += 1;
                }
            }

            return new string(chars.ToArray());
        }

        public static bool IsValidInt(string s) => IntRegex.IsMatch(s);

        public static bool IsCommentStart(char c) => c == CommentIdentifier;

        public static bool IsCommentStop(string s, int i) => IsUnescapedStop(s, i, CommentIdentifier);

        public static bool IsBracket(char c) => c == '{' || c == '}' || c == '[' || c == ']';

        public static bool IsWhiteSpace(char c) => c == ' ' || c == '\t' || c == '\n';

        public static bool IsCharToSkip(char c) => c == '\r';

        public static bool IsUnescapedStop(string s, int i, char t)
        {
            if (i < 0 || i >= s.Length) return false;

            if (s[i] != t) return false;

            var countOfEscapes = 0;

            for (var j = i - 1; j >= 0; --j)
            {
                if (s[j] == '\\')
                    countOfEscapes += 1;
                else
                    break;
            }

            return countOfEscapes % 2 == 0;
        }
    }
}
