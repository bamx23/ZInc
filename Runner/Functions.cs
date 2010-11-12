using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Runner
{
    public class BinaryOperatorObj // оператор который принимает два значения и возвращает один, думаю надо инкапсулировать в класс, чтобы потом не было проблем с добавлением пользовательских операторов
    { // Хотя в общем не знаю что сюда писать, так что оставлю пока так, напишу без этой мути, потом если что перепишу =)
        public BinaryOperatorObj(string rhsName)
        {
            name = rhsName;
        }
        protected string name;
        protected string param1;
        protected string param2;
    }
    public class ExprObject
    {
        protected string name;
        public string Name
        {
            get { return name; }
        }

        public List<string> param;
        public string Err;
        protected Runing Prog;

        public virtual int Do(ref int Line)
        {
            Line++;
            return 0;
        }

        public virtual ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprObject e =  new ExprObject(Prog);
            e.param = Param;
            return e;
        }

        public virtual int Except(Exception e)
        {
            Err = e.Message;
            return 1; 
        }

        public ExprObject(Runing Prog)
        {
            this.Prog = Prog;
            param = new List<string>();
        }
    }

    public class ExprSet : ExprObject
    {
        public override int Do(ref int Line) // Коля, а чего у тебя только інт возвращается? а если нужен дабл // Где?)) Это номер строки
        {
            try
            {
                VarObject V = Prog.GetVar(param[0]);
                switch (V.Type)
                {
                    case Constants.INT:
                        ((VarInt)V).data = Prog.GetValInt(param[1]);
                        break;
                    case Constants.DOUBLE:
                        ((VarDouble)V).data = Prog.GetValDouble(param[1]);
                        break;
                    case Constants.STRING:
                        ((VarString)V).data = Prog.GetValString(param[1]);
                        break;
                    case Constants.BOOL:
                        ((VarBool)V).data = Prog.GetValBool(param[1]);
                        break;
                }
                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }

        public override ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprSet e = new ExprSet(Prog);
            e.param = Param;
            return e;
        }
        
        public ExprSet(Runing Prog) : base(Prog)
        {
            name = "set";
        }
    }

    public class ExprIn : ExprObject
    {
        public override int Do(ref int Line)
        {
            try
            {
                switch(param[0])
                {
                    case "std":
                        for (int i = 1; i < param.Count(); i++)
                            Prog.GetVar(param[i]).Parse(Prog.stdIO.In());
                        break;
                    default: // Here will be some code i think - Really?
                        break;
                }
                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }

        public override ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprIn e = new ExprIn(Prog);
            e.param = Param;
            return e;
        }

        public ExprIn(Runing Prog) : base(Prog)
        {
            name = "in";
        }
    }

    public class ExprOut : ExprObject
    {
        public override int Do(ref int Line)
        {
            try
            {
                string R = "";
                for (int i = 1; i < param.Count(); i++)
                    R += Prog.GetValString(param[i]);

                switch (param[0])
                {
                    case "std":
                        Prog.stdIO.Out(R);
                        break;
                }

                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }

        public override ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprOut e = new ExprOut(Prog);
            e.param = Param;
            return e;
        }

        public ExprOut(Runing Prog) : base(Prog)
        {
            name = "out";
        }
    }

    public class ExprInc : ExprObject
    {
        public override int Do(ref int Line)
        {
            try
            {
                ((VarInt)Prog.GetVar(param[0])).data++;

                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }

        public override ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprInc e = new ExprInc(Prog);
            e.param = Param;
            return e;
        }

        public ExprInc(Runing Prog) : base(Prog)
        {
            name = "inc";
        }
    }

    public class ExprIfgo : ExprObject
    {
        public override int Do(ref int Line)
        {
            try
            {
                if (Prog.GetValBool(param[0]))
                    Line = int.Parse(param[1]);
                else
                    Line = int.Parse(param[2]);

                return 0;
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }

        public override ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprIfgo e = new ExprIfgo(Prog);
            e.param = Param;
            return e;
        }

        public ExprIfgo(Runing Prog) : base(Prog)
        {
            name = "ifgo";
        }
    }

    public class ExprReturn : ExprObject
    {
        public override int Do(ref int Line)
        {
            try
            {
                Line = -1;
                return 0;
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }

        public override ExprObject Clone(Runing Prog, List<string> Param)
        {
            ExprReturn e = new ExprReturn(Prog);
            e.param = Param;
            return e;
        }

        public ExprReturn(Runing Prog) : base(Prog)
        {
            name = "return";
        }
    }

       
}


/* public override int Do(ref int Line)
        {
            try
            {
                // CODE over here
                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
        }
*/