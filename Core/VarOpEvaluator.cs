namespace StackScript0.Core
{
    public class VarOpEvaluator
    {
        public static bool IsTrue(Token v)
        {
            switch (v.Type)
            {
                case TokenType.NONE:
                    return false;
                case TokenType.INT:
                    return ((VariableInt)v).Value != 0;
                case TokenType.FLOAT:
                    return ((VariableFloat)v).Value != 0.0;
                case TokenType.STRING:
                    return ((VariableString)v).Value.Length > 0;
                default:
                    return true;
            }
        }

        public static Token ToTypeString(Token v) =>
            new VariableString(v.Type.ToString());

        public static Token ToInt(Token v)
        {
            switch (v.Type)
            {
                case TokenType.INT:
                    return v.GetClone();
                case TokenType.FLOAT:
                    return new VariableInt((long)((VariableFloat)v).Value);
                case TokenType.STRING:
                    return long.TryParse(((VariableString)v).Value, out long l)
                        ? new VariableInt(l)
                        : new VariableInt(0);
                default:
                    return new VariableNone();
            }
        }

        public static Token ToFloat(Token v)
        {
            switch (v.Type)
            {
                case TokenType.INT:
                    return new VariableFloat(((VariableInt)v).Value);
                case TokenType.FLOAT:
                    return v.GetClone();
                case TokenType.STRING:
                    return double.TryParse(((VariableString)v).Value, out double d)
                        ? new VariableFloat(d)
                        : new VariableFloat(0.0);
                default:
                    return new VariableNone();
            }
        }

        public static Token ToString(Token v)
        {
            switch (v.Type)
            {
                case TokenType.INT:
                case TokenType.FLOAT:
                    return new VariableString(v.ToStringToken(0));
                case TokenType.STRING:
                    return v.GetClone();
                default:
                    return new VariableNone();
            }
        }

        public static Token Not(Token v)
        {
            switch (v.Type)
            {
                case TokenType.INT:
                    return v.IsEqual(new VariableInt(0))
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.FLOAT:
                    return v.IsEqual(new VariableFloat(0.0))
                        ? new VariableFloat(1.0)
                        : new VariableFloat(0.0);
                case TokenType.STRING:
                    return v.IsEqual(new VariableString(""))
                        ? new VariableString("1")
                        : new VariableString("");
                default:
                    return new VariableNone();
            }
        }

        public static Token Or(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return ri.Value > li.Value
                        ? new VariableInt(ri.Value)
                        : new VariableInt(li.Value);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return rf.Value > lf.Value
                        ? new VariableFloat(rf.Value)
                        : new VariableFloat(lf.Value);
                default:
                    return new VariableNone();
            }
        }

        public static Token And(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return ri.Value < li.Value
                        ? new VariableInt(ri.Value)
                        : new VariableInt(li.Value);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return rf.Value < lf.Value
                        ? new VariableFloat(rf.Value)
                        : new VariableFloat(lf.Value);
                default:
                    return new VariableNone();
            }
        }

        public static Token Add(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return new VariableInt(li.Value + ri.Value);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return new VariableFloat(lf.Value + rf.Value);
                case TokenType.STRING:
                    var ls = (VariableString)l;
                    var rs = (VariableString)r;
                    return new VariableString(ls.Value + rs.Value);
                default:
                    return new VariableNone();
            }
        }

        public static Token Subtract(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return new VariableInt(li.Value - ri.Value);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return new VariableFloat(lf.Value - rf.Value);
                case TokenType.STRING:
                    var ls = (VariableString)l;
                    var rs = (VariableString)r;
                    var ts = new VariableString(ls.Value);

                    foreach (char c in rs.Value)
                    {
                        var lastIndex = ls.Value.LastIndexOf(c);
                        if (lastIndex < 0) continue;
                        ts.Value = ls.Value.Remove(lastIndex, 1);
                    }

                    return ts;
                default:
                    return new VariableNone();
            }
        }

        public static Token Multiply(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return new VariableInt(li.Value * ri.Value);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return new VariableFloat(lf.Value * rf.Value);
                default:
                    return new VariableNone();
            }
        }

        public static Token Divide(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return new VariableInt(li.Value / ri.Value);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return new VariableFloat(lf.Value / rf.Value);
                default:
                    return new VariableNone();
            }
        }

        public static Token Modulus(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return new VariableInt(li.Value % ri.Value);
                default:
                    return new VariableNone();
            }
        }

        public static Token Higher(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return li.Value > ri.Value
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return lf.Value > rf.Value
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.STRING:
                    var ls = (VariableString)l;
                    var rs = (VariableString)r;
                    return string.Compare(ls.Value, rs.Value) == 1
                        ? new VariableInt(1)
                        : new VariableInt(0);
                default:
                    return new VariableNone();
            }
        }

        public static Token Lower(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return li.Value < ri.Value
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return lf.Value < rf.Value
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.STRING:
                    var ls = (VariableString)l;
                    var rs = (VariableString)r;
                    return string.Compare(ls.Value, rs.Value) == -1
                        ? new VariableInt(1)
                        : new VariableInt(0);
                default:
                    return new VariableNone();
            }
        }

        public static Token Equal(Token l, Token r)
        {
            switch (l.Type)
            {
                case TokenType.INT:
                    var li = (VariableInt)l;
                    var ri = (VariableInt)r;
                    return li.Value == ri.Value
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.FLOAT:
                    var lf = (VariableFloat)l;
                    var rf = (VariableFloat)r;
                    return lf.Value == rf.Value
                        ? new VariableInt(1)
                        : new VariableInt(0);
                case TokenType.STRING:
                    var ls = (VariableString)l;
                    var rs = (VariableString)r;
                    return string.Compare(ls.Value, rs.Value) == 0
                        ? new VariableInt(1)
                        : new VariableInt(0);
                default:
                    return new VariableNone();
            }
        }
    }
}
