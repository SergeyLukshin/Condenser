using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Condenser
{
    public class Lexems
    {
        public enum LexemType
        {
            Variable,
            Function,
            Identifier,
            Operator,
            Delimier,
            Constant,
            OpenSub,
            CloseSub
        }

        public class Lexem
        {
            public LexemType Type { get; set; }
            public string Value { get; set; }

            /*public override string ToString()
            {
                return "{0} : {1}".Format(Type, Value);
            }*/
        }

        public List<Lexem> GetAst(string expr)
        {
            List<Lexem> lexems = new List<Lexem>();
            string acc = "";
            bool readingId = false;
            LexemType nextType = LexemType.Constant;
            for (int i = 0; i < expr.Length; ++i)
            {
                char c = expr[i];
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else
                    if (char.IsLetter(c))
                    {
                        if (!readingId)
                        {
                            nextType = LexemType.Identifier;
                            readingId = true;
                        }
                        if (c == '.') throw new Exception("Unexpected char!");
                        acc += c;
                    }
                    else if (char.IsDigit(c))
                    {
                        if (!readingId)
                        {
                            nextType = LexemType.Constant;
                            readingId = true;
                        }
                        acc += c;
                    }
                    else if (c == '.' && readingId && nextType == LexemType.Constant)
                        acc += '.';
                    else if (char.IsLetterOrDigit(c)) acc += c;
                    else if ((c == '+' || c == '-') && (i > 0 && (expr[i - 1] == '+' || expr[i - 1] == '-')) && char.IsDigit(expr[i + 1]) && !readingId)
                    {
                        nextType = LexemType.Constant;
                        readingId = true;
                        acc += c;
                    }
                    else if (c == '(' || c == ')' || c == '+' || c == '-' || c == '*' || c == '/' || c == '^')
                    {
                        if (readingId)
                        {
                            readingId = false;
                            if (c == '(' && nextType == LexemType.Identifier) nextType = LexemType.Function;
                            else if (c != '(' && nextType == LexemType.Identifier) nextType = LexemType.Variable;
                            lexems.Add(new Lexem { Value = acc, Type = nextType });
                            acc = "";
                        }
                        lexems.Add(new Lexem { Value = c.ToString(), Type = LexemType.Operator });
                    }
                    else if (c == ',')
                    {
                        if (readingId)
                        {
                            readingId = false;
                            if (c == '(' && nextType == LexemType.Identifier) nextType = LexemType.Function;
                            else if (c != '(' && nextType == LexemType.Identifier) nextType = LexemType.Variable;
                            lexems.Add(new Lexem { Value = acc, Type = nextType });
                            acc = "";
                        }
                        lexems.Add(new Lexem { Value = c.ToString(), Type = LexemType.Delimier });
                    }
            }

            if (acc != "" && nextType == LexemType.Identifier)
            {
                nextType = LexemType.Variable;
                lexems.Add(new Lexem { Value = acc, Type = nextType });
            }
            else lexems.Add(new Lexem { Value = acc, Type = nextType });
            return lexems;
        }

        public int GetPriority(Lexem l)
        {
            if (l.Type == LexemType.Operator ||
                l.Type == LexemType.Function)
            {
                if (l.Value == "+" || l.Value == "-") return 1;
                else if (l.Value == "*" || l.Value == "/") return 2;
                else if (l.Value == "^") return 3;
                else if (l.Value == "(") return 5;
                else return 4;
            }
            return -1;
        }

        public class Tuple
        {
            public string param1;
            public double param2;
        }

        public class Tuple2
        {
            public Tuple2(double p1, double p2)
            {
                param1 = p1;
                param2 = p2;
            }

            public double param1;
            public double param2;
        }

        public class RPN
        {
            public Dictionary<string, Tuple2> Variables { get; set; }
            public Dictionary<string, FunctionBase> Functions { get; set; }
            public Stack<double> Stack { get; set; }
            public string Expression { get; set; }
            public List<Lexem> Ast { get; private set; }
            public double Result { get; private set; }

            /*public double this[params Tuple[] args]
            {
                get
                {
                    RPN r = this.MemberwiseClone() as RPN;
                    foreach (var arg in args) r.SetVariable(arg.param1, arg.param2);
                    r.Execute();
                    return r.Result;
                }
            }*/

            public RPN(string expr)
            {
                this.Variables = new Dictionary<string, Tuple2>();
                this.Functions = new Dictionary<string, FunctionBase>();
                this.Stack = new Stack<double>();
                this.Expression = expr;
            }

            private bool IsNumber(string s)
            {
                //return Regex.IsMatch(s, @"[+-]?\d+(\.\d+)?", RegexOptions.Compiled);
                double val;
                return double.TryParse(s, out val);
            }
            private bool IsVariable(string s)
            {
                return Regex.IsMatch(s, @"[$_a-zA-Z]+[$_a-zA-Z0-9]*", RegexOptions.Compiled);
            }
            private bool IsOperation(string s)
            {
                return (s == "+" || s == "-" || s == "*" || s == "/" || s == "^");
            }
            private bool IsDelimier(string s)
            {
                return s == ",";
            }
            private bool IsFunction(string s)
            {
                return HasFunction(s);
            }

            public void CreateAst()
            {
                string[] t = Expression.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Ast = new List<Lexem>();
                foreach (var l in t)
                {
                    if (IsNumber(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Constant });
                    else if (IsFunction(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Function });
                    else if (IsVariable(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Variable });
                    else if (IsOperation(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Operator });
                    else if (IsDelimier(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Delimier });
                    else throw new Exception("Unexpected lexem!");
                }
            }

            public void SetVariable(string name, double value, double def_val = double.NaN)
            {
                if (HasVariable(name.ToLower()))
                {
                    Variables[name.ToLower()] = new Tuple2(value, def_val);
                }
                else Variables.Add(name.ToLower(), new Tuple2(value, def_val));
            }

            public void AddFunction(FunctionBase func)
            {
                Functions.Add(func.Name, func);
            }

            public bool HasVariable(string name)
            {
                return Variables.ContainsKey(name.ToLower());
            }

            public bool HasFunction(string name)
            {
                return Functions.ContainsKey(name.ToLower());
            }

            public void Execute()
            {
                foreach (var l in Ast)
                {
                    switch (l.Type)
                    {
                        case LexemType.Constant: Stack.Push(double.Parse(l.Value)); break;
                        case LexemType.Variable:
                            {
                                if (!double.IsNaN(Variables[l.Value].param1))
                                    Stack.Push(Variables[l.Value].param1);
                                else
                                    Stack.Push(Variables[l.Value].param2);
                                break;
                            }
                        case LexemType.Function:
                            {
                                var func = Functions[l.Value];
                                var args = new List<double>();
                                for (int i = 0; i < func.NeededArgs; ++i) args.Add(Stack.Pop());
                                Stack.Push(func.Execute(args.ToArray()));
                            } break;
                        case LexemType.Operator: Stack.Push(BasicFunction(l.Value)); break;
                        default: throw new Exception("Unexpected lexem!");
                    }
                }
                Result = Stack.Pop();
            }

            public double BasicFunction(string f)
            {
                switch (f)
                {
                    case "+":
                        return Stack.Pop() + Stack.Pop();
                    case "-":
                        {
                            var t1 = Stack.Pop();
                            var t2 = Stack.Pop();
                            return t2 - t1;
                        }
                    case "*":
                        return Stack.Pop() * Stack.Pop();
                    case "/":
                        {
                            var t1 = Stack.Pop();
                            var t2 = Stack.Pop();
                            return t2 / t1;
                        }
                    case "^":
                        {
                            var t1 = Stack.Pop();
                            var t2 = Stack.Pop();
                            return Math.Pow(t2, t1);
                        }
                    default: return double.NaN;
                }
            }
        }

        public abstract class FunctionBase
        {
            public virtual string Name { get; set; }
            public virtual int NeededArgs { get; set; }

            public virtual double Execute(params double[] args)
            {
                return double.NaN;
            }
        }

        /*public class NativeFunction : FunctionBase
        {
            public static NativeFunction Sin
            {
                get
                {
                    return new NativeFunction("sin", a => Math.Sin(a[0]), 1);
                }
            }
            public static NativeFunction Cos
            {
                get
                {
                    return new NativeFunction("cos", a => Math.Cos(a[0]), 1);
                }
            }
            public static NativeFunction Sqrt
            {
                get
                {
                    return new NativeFunction("sqrt", a => Math.Sqrt(a[0]), 1);
                }
            }
            public static NativeFunction Exp
            {
                get
                {
                    return new NativeFunction("exp", a => Math.Exp(a[0]), 1);
                }
            }
            public static NativeFunction Abs
            {
                get
                {
                    return new NativeFunction("abs", a => Math.Abs(a[0]), 1);
                }
            }
            public static NativeFunction Ln
            {
                get
                {
                    return new NativeFunction("ln", a => Math.Log10(a[0]), 1);
                }
            }
            public static NativeFunction Lb
            {
                get
                {
                    return new NativeFunction("lb", a => Math.Log(a[0], 2.0), 1);
                }
            }
            public static NativeFunction Log
            {
                get
                {
                    return new NativeFunction("log", a => Math.Log(a[0], a[1]), 2);
                }
            }
            public static NativeFunction Pow
            {
                get
                {
                    return new NativeFunction("pow", a => Math.Pow(a[0], a[1]), 2);
                }
            }
            public static NativeFunction Atan
            {
                get
                {
                    return new NativeFunction("atan", a => Math.Atan(a[0]), 1);
                }
            }

            public override string Name { get; set; }
            public Func<double[], double> Function { get; set; }
            public override int NeededArgs { get; set; }

            public NativeFunction(string name, Func<double[], double> function, int neededArgs)
            {
                this.Name = name;
                this.Function = function;
                this.NeededArgs = neededArgs;
            }

            public override double Execute(params double[] args)
            {
                if (Function != null) return Function(args);
                return double.NaN;
            }
        }*/

        /*public class RPNFunction : FunctionBase
        {
            public override string Name { get; set; }
            public RPN Function { get; set; }
            public override int NeededArgs { get; set; }

            public void UpdateExpression(string newExpr, int newNeededArgs)
            {
                this.NeededArgs = newNeededArgs;
                this.Function.Expression = newExpr;
            }

            public RPNFunction(string name, RPN function, int neededArgs)
            {
                this.Name = name;
                this.Function = function;
                this.NeededArgs = neededArgs;
            }

            public override double Execute(params double[] args)
            {
                if (this.Function == null) return double.NaN;
                if (args.Length != this.NeededArgs) return double.NaN;
                for (int i = 0; i < args.Length; ++i)
                    this.Function.SetVariable(string.Format("${0}", i + 1), args[i]);
                this.Function.Execute();
                return this.Function.Result;
            }

            public override string ToString()
            {
                return this.Function.Expression;
            }
        }*/

        public string StandartToRPN(string expr)
        {
            var ast = GetAst(expr);
            Stack<Lexem> operators = new Stack<Lexem>();
            List<string> result = new List<string>();
            foreach (var l in ast)
            {
                if (l.Type == LexemType.Variable || l.Type == LexemType.Constant)
                    result.Add(l.Value);
                else if (l.Type == LexemType.Operator && l.Value == ")")
                {
                    Lexem l0 = new Lexem();
                    while ((l0 = operators.Pop()).Value != "(")
                        if (l0.Value != ",")
                            result.Add(l0.Value);
                }
                else if (l.Type == LexemType.Operator || l.Type == LexemType.Function)
                {
                    if (operators.Count > 0 && GetPriority(operators.Peek()) == GetPriority(l))
                    {
                        var l0 = operators.Pop();
                        if (l0.Value != ",") result.Add(l0.Value);
                    }
                    operators.Push(l);
                }
            }
            while (operators.Count > 0)
            {
                var l0 = operators.Pop();
                if (l0.Value != ",") result.Add(l0.Value);
            }
            string rpnExpr = StringJoin(" ", result);
            return rpnExpr;
        }

        private string StringJoin(string strDelimiter, List<string> arr)
        {
            string strResult = arr[0];
            for (int i = 1; i < arr.Count; i++)
            {

                strResult = strResult + strDelimiter + arr[i];
            }
            return strResult;
        }

        public RPN CreateRPN(string expr)
        {
            RPN rpn = new RPN(expr);
            rpn.CreateAst();
            return rpn;
        }
    }
}