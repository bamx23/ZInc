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
        protected int ParamCount;
        public IO stdIO;

        public bool Halt = false;
        public event ProcessMessages ProcMess;

        public List<ExprObject> Lines;

        public VarObject GetVar(string name)
        {
            //Обнаружение скобок и рекуретный вызов GetVar() - заменяем все скобки их значениями
            if(name.IndexOf('(') != -1)
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
            switch(name[0])
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

        public Runing(IO stdIO)
        {
            Var = new List<VarObject>();
            Temp = new VarInt(0);
            ParamCount = 0;
            this.stdIO = stdIO;
        }

        public Runing(IO stdIO, List<VarObject> Params)
        {
            Var = new List<VarObject>();
            Temp = new VarInt(0);
            Param = Params;
            ParamCount = Params.Count;
            this.stdIO = stdIO;
        }

        public void Test()
        {
            //Проверка работы переменных
            Var.Add(new VarInt(1));
            Var.Add(new VarInt(11));
            stdIO.Out("Введите 5(число будет записано в V2):");
            //Ввод данных \/ и вывод данных /\
            string s = stdIO.In();
            Var.Add(new VarInt(int.Parse(s)));
            Var.Add(new VarInt(1));
            ((VarInt)Var[0][5][11]).data = 8;

            //Проверка event'а ProcMess
            stdIO.Out("Идет проверка ProcMess()... подождите");
            for (int i = 0; i < 10000000; i++)
            {
                s = i.ToString();
                if(ProcMess != null)
                    ProcMess();
            }

            stdIO.Out("В переменной V0.(V2).(V(V3)): "+GetVar("V0.(V2).(V(V3))").ToStr());

            return;
        }

        public void Run()
        {
            int Cur = 0;
            bool Err = false;
            while (Cur <= Lines.Count() && (Err == false))
            {
                Err = (Lines[Cur].Do(ref Cur) != 0);

                if (Cur == -1)
                    return;

                if (ProcMess != null)
                    ProcMess();

                if (Halt)
                    return;
            }
            if (Err)
            {
                stdIO.Out(Lines[Cur].Err);
            }
        }
    }
}
