using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Runner
{
    public class IO
    {
        public virtual void Out(string s)
        {
        }

        public virtual string In()
        {
            return "ERROR";
        }
    }

    public delegate void ProcessMessages();

    public class Runing
    {
        public List<VarObject> Var;
        public VarInt Temp;
        protected List<VarObject> Param;
        protected List<string> Source;
        protected int ParamCount;
        public IO stdIO;

        public List<ExprObject> Expressions;

        public bool Halt = false;
        public event ProcessMessages ProcMess;

        public List<ExprObject> Lines;

        /// <summary>
        /// Ну по идее этот метод находит переменную по ее "адресу" или как там его
        /// </summary>
        /// <param name="name">строка в которой лежит эта самая переменная</param>
        /// <returns>возвращает саму переменную</returns>
        public VarObject GetVar(string name)
        {
            //Обнаружение скобок и рекуретный вызов GetVar() - заменяем все скобки их значениями
            if (name.IndexOf('(') != -1)
            {
                Regex o = new Regex(@"\((?=[^\(]*\)).*?\)");
                try
                {
                    while (o.IsMatch(name))
                        name = o.Replace(name, GetVar(o.Match(name).Value.Trim('(', ')')).ToStr(), 1);
                }
                catch (Exception e)
                {
                    //Обработка ошибок
                }
            }

            MatchCollection r = (new Regex(@"\d+")).Matches(name);
            VarObject R;

            //Разные типы переменных: V - обычный Var, T - темповая, P - параметр
            switch (name[0])
            {
                case 'V':
                    R = Var[int.Parse(r[0].Value)];

                    for (int i = 1; i < r.Count; i++)
                        R = R[int.Parse(r[i].Value)];

                    return R;
                case 'T':
                    R = Temp;

                    for (int i = 0; i < r.Count; i++)
                        R = R[int.Parse(r[i].Value)];

                    return R;
                case 'P':
                    R = Param[int.Parse(r[0].Value)];

                    for (int i = 1; i < r.Count; i++)
                        R = R[int.Parse(r[i].Value)];

                    return R;
                default:
                    return null;
            }
        }
        public int GetValInt(string name)
        {
            if (name[0] == '(')
            {
                name = name.Remove(0, 1);
                name = name.Remove(name.Length - 1, 1);
            }
            return ParseString(name).ToInt();
        } // "getVal второй параметр ссылка на переменную куда мы запишем результат" можно и так переделать это не сложно, если будет удобнее скажите, сделаю, ну или сами потужтесь
        public bool GetValBool(string name)
        {
            if (name[0] == '(')
            {
                name = name.Remove(0, 1);
                name = name.Remove(name.Length - 1, 1);
            }
            return ParseString(name).ToBool();
        }
        public double GetValDouble(string name)
        {
            if (name[0] == '(')
            {
                name = name.Remove(0, 1);
                name = name.Remove(name.Length - 1, 1);
            }
            return ParseString(name).ToDouble();
        }
        public string GetValString(string name)
        {
            //name = name.Trim('(', ')'); // Плохо! В (V1.(V2)) -> V1.(V2 =)
            if (name[0] == '(')
            {
                name = name.Remove(0, 1);
                name = name.Remove(name.Length - 1, 1);
            }
            return ParseString(name).ToStr();
        }
        /// <summary>
        /// Самым непосредственным образом парсит строку записанную польской записью =)
        /// </summary>
        /// <param name="name">собственно сама строка, которую надо разбрать</param>
        /// <returns></returns>
        public VarObject ParseString(string name)
        {
            Stack<VarObject> operands = new Stack<VarObject>();
            string[] literal = name.Split(' ');
            foreach (string s in literal)
            {
                if (s[0] == 'V' || s[0] == 'P' || s[0] == 'T')
                {
                    try
                    {
                        operands.Push(GetVar(s));
                    }
                    catch (Exception e)
                    {
                    }
                }
                else
                {
                    if (s == "+" || s == "-" || s == "*" || s == "/" ||
                        s == "==" || s == "<" || s == "<=" || s == ">" || s == ">=")
                        ConvertToOneType(operands, s);
                    else
                    {
                        if (s[0] == '\'' && s[s.Length - 1] == '\'')
                        {
                            operands.Push(new VarString(s.Trim('\'', '\'')));
                            continue;
                        }
                        if (s[0] == '\"' && s[s.Length - 1] == '\"')
                        {
                            operands.Push(new VarString(s.Trim('\"', '\"')));
                            continue;
                        }
                        if (s == "true" || s == "false")
                        {
                            operands.Push(new VarBool(bool.Parse(s)));
                            continue;
                        }
                        double dNum;
                        int lNum;
                        if (double.TryParse(s, out dNum))
                        {
                            operands.Push(new VarDouble(dNum));
                            continue;
                        }
                        if (int.TryParse(s, out lNum))
                        {
                            operands.Push(new VarInt(lNum));
                            continue;
                        }
                    }
                }
            }
            return operands.Pop();
        }
        /// <summary>
        /// Этот метод приводит две переменные к одному типу, а потом колбасит их не по детски, и выполняет над ними операцию
        /// </summary>
        /// <param name="operands"> стек операндов текущего выражения</param>
        /// <param name="operation">А это кстати строка в которой лежит текущая операция, подход не супер, но лучше чем ничего</param>
        public void ConvertToOneType(Stack<VarObject> operands, string operation)
        {
            VarObject a, b;
            VarBool operBool1, operBool2;
            VarInt operInt1, operInt2;
            VarDouble operDouble1, operDouble2;
            VarString operString1, operString2;
            b = operands.Pop(); // Тут важен порядок поп(б) поп(а) не наоборот, иначе со стрингами лажа: задом наперед стринги получаются =)
            a = operands.Pop();
            switch (Math.Max(a.Type, b.Type))
            {
                case Constants.BOOL:
                    operBool1 = new VarBool(a.ToBool());
                    operBool2 = new VarBool(b.ToBool());
                    operands.Push(DoOperation(operBool1, operBool2, operation));
                    break;
                case Constants.INT:
                    operInt1 = new VarInt(a.ToInt());
                    operInt2 = new VarInt(b.ToInt());
                    operands.Push(DoOperation(operInt1, operInt2, operation));
                    break;
                case Constants.DOUBLE:
                    operDouble1 = new VarDouble(a.ToDouble());
                    operDouble2 = new VarDouble(b.ToDouble());
                    operands.Push(DoOperation(operDouble1, operDouble2, operation));
                    break;
                case Constants.STRING:
                    operString1 = new VarString(a.ToStr());
                    operString2 = new VarString(b.ToStr());
                    operands.Push(DoOperation(operString1, operString2, operation));
                    break;
                default: // ololo Oshibka detected what to do now???? todolist!!!! oshibka oshibka( tut proishodit beg po krugu i kriki o pomoshi)
                    break;
            }
        }
        public VarObject DoOperation(VarInt a, VarInt b, string operation)
        {
            if (operation == "+" || operation == "-" || operation == "*" || operation == "/")
                return DoArifmeticOperation(a, b, operation);
            else
                return DoLogicalOperation(a, b, operation);
        }
        public VarObject DoOperation(VarBool a, VarBool b, string operation)
        {
            if (operation == "+" || operation == "-" || operation == "*" || operation == "/")
                return DoArifmeticOperation(a, b, operation);
            else
                return DoLogicalOperation(a, b, operation);
        }
        public VarObject DoOperation(VarDouble a, VarDouble b, string operation)
        {
            if (operation == "+" || operation == "-" || operation == "*" || operation == "/")
                return DoArifmeticOperation(a, b, operation);
            else
                return DoLogicalOperation(a, b, operation);
        }
        public VarObject DoOperation(VarString a, VarString b, string operation)
        {
            if (operation == "+" || operation == "-" || operation == "*" || operation == "/")
                return DoArifmeticOperation(a, b, operation);
            else
                return DoLogicalOperation(a, b, operation);
        }

        public VarInt DoArifmeticOperation(VarInt a, VarInt b, string operation)
        {
            switch (operation)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    break;
            }
            return null;
        }
        public VarBool DoArifmeticOperation(VarBool a, VarBool b, string operation)
        {
            switch (operation)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    break;
            }
            return null;
        }
        public VarDouble DoArifmeticOperation(VarDouble a, VarDouble b, string operation)
        {
            switch (operation)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    break;
            }
            return null;
        }
        public VarString DoArifmeticOperation(VarString a, VarString b, string operation)
        {
            switch (operation)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    break;
            }
            return null;
        }

        public VarBool DoLogicalOperation(VarInt a, VarInt b, string operation)
        {
            switch (operation)
            {
                case "<":
                    return new VarBool(a < b);
                case ">":
                    return new VarBool(a > b);
                case "<=":
                    return new VarBool(a <= b);
                case ">=":
                    return new VarBool(a >= b);
                default:
                    break;
            }
            return null;
        }
        public VarBool DoLogicalOperation(VarBool a, VarBool b, string operation)
        {
            switch (operation)
            {
                case "<":
                    return new VarBool(a < b);
                case ">":
                    return new VarBool(a > b);
                case "<=":
                    return new VarBool(a <= b);
                case ">=":
                    return new VarBool(a >= b);
                default:
                    break;
            }
            return null;
        }
        public VarBool DoLogicalOperation(VarDouble a, VarDouble b, string operation)
        {
            switch (operation)
            {
                case "<":
                    return new VarBool(a < b);
                case ">":
                    return new VarBool(a > b);
                case "<=":
                    return new VarBool(a <= b);
                case ">=":
                    return new VarBool(a >= b);
                default:
                    break;
            }
            return null;
        }
        public VarBool DoLogicalOperation(VarString a, VarString b, string operation)
        {
            switch (operation)
            {
                case "<":
                    return new VarBool(a < b);
                case ">":
                    return new VarBool(a > b);
                case "<=":
                    return new VarBool(a <= b);
                case ">=":
                    return new VarBool(a >= b);
                default:
                    break;
            }
            return null;
        }

        public void Test()
        {
            //Проверка работы переменных
            Var.Add(new VarInt(3));
            Var.Add(new VarInt(11));

            stdIO.Out("Введите 5(число будет записано в V2):");

            //Ввод данных \/ и вывод данных /\
            string s = stdIO.In();
            Var.Add(new VarInt(int.Parse(s)));
            Var.Add(new VarInt(1));
            Var.Add(new VarString("Hello"));
            Var.Add(new VarString(" World!"));
            GetValInt("(V1 V3 +)");
            ((VarInt)Var[0][5][11]).data = 8;

            ExprOut E = new ExprOut(this);

            E.param.Add("std"); E.param.Add("(V2 V3 +)");
            //E.param.Add("(V2 V3 +)");
            int L = 0;
            L = E.Do(ref L);

            //Проверка event'а ProcMess
            /*stdIO.Out("Идет проверка ProcMess()... подождите");
            for (int i = 0; i < 10000000; i++) // Коля, а что это за цикл расскажи сне? =)
            {
                s = i.ToString();
                if(ProcMess != null)
                    ProcMess();
            }*/

            stdIO.Out("В переменной V0.(V2).(V(V3)): " + GetVar("V0.(V2).(V(V3))").ToStr());

            return;
        }

        //Убрал, т.к. раньше вызывали конструктор без содержимого, которое надо выполнить. А сейчас так ни-ни!
        //public Runing(IO stdIO)
        //{
        //    Var = new List<VarObject>();
        //    Temp = new VarInt(0);
        //    ParamCount = 0;
        //    this.stdIO = stdIO;
        //}

        public Runing(IO stdIO, List<string> lSource)
        {
            Var = new List<VarObject>();
            Temp = new VarInt(0);
            FormExprList();
            Source = lSource;
            Param = new List<VarObject>();
            ParamCount = Param.Count;
            this.stdIO = stdIO;
        }

        public Runing(IO stdIO, List<string> lSource, List<VarObject> Params)
        {
            Var = new List<VarObject>();
            Temp = new VarInt(0);
            FormExprList();
            Source = lSource;
            Param = Params;
            ParamCount = Params.Count;
            this.stdIO = stdIO;
        }

        private void FormExprList()
        {
            Expressions = new List<ExprObject>();
            Expressions.Add(new ExprIn(null));
            Expressions.Add(new ExprOut(null));
            Expressions.Add(new ExprInc(null));
            Expressions.Add(new ExprIfgo(null));
            Expressions.Add(new ExprSet(null));
            Expressions.Add(new ExprReturn(null));
            Expressions.Add(new ExprNew(this));
        }

        public void Run()
        {
            fillLines();
            int Cur = 0;
            bool Err = false;
            while (Cur < Lines.Count() && (Err == false))
            {
                Err = (Lines[Cur].Do(ref Cur) != 0);

                if (Cur == -1)
                    return;
            }
            if (Err)
            {
                stdIO.Out(Lines[Cur].Err);
            }
        }

        private void fillLines()
        {
            if (Lines == null) Lines = new List<ExprObject>();
            Lines.Clear();

            foreach (string s in Source)
            {
                Regex o = new Regex(@"(\(.+?\))|(\'.{1}?\')|(\"".*?\"")|([\w=<>]+)");
                //RegExp, который выделяет выражения в скобках, в одинарных и двойных кавычках, а оставшееся делит на слова (которые состоят из букав и = < >)

                MatchCollection r = o.Matches(s);
                //r[0] - носер строки кода программы из которой была полученна данная прекомпильная конструкция - т.е. #i
                List<string> sPar = new List<string>();
                for (int i = 2; i < r.Count; i++) sPar.Add(r[i].ToString());
                Lines.Add(CreateExprObj(r[1].ToString(), sPar));
            }
        }

        private ExprObject CreateExprObj(string precompName, List<string> sPar)
        {
            //если придумаем, тут будет более расширяемая конструкция
            foreach (ExprObject expr in Expressions)
            {
                if (expr.Name == precompName) return expr.Clone(this, sPar);
            }
            return new ExprObject(this);
        }
    }
}
