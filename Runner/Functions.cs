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

        public virtual int Except(Exception e)
        {
            Err = e.Message;
            return 1; 
        }

        public ExprObject(Runing Prog)
        {
            this.Prog = Prog;
        }
    }

    public class ExprSet : ExprObject
    {
        public override int Do(ref int Line) // Коля, а чего у тебя только інт возвращается? а если нужен дабл
        {
            try
            {
                VarObject V = Prog.GetVar(param[0]);
                switch (V.Type)
                {
                    case 1:
                        //Prog.GetVal(param[1],((VarInt)V).data);
                        break;
                    case 2:
                        //Prog.GetVal(param[1],((VarDouble)V).data);
                        break;
                    case 3:
                        //Prog.GetVal(param[1],((VarString)V).data);
                        break;
                    case 4:
                        //Prog.GetVal(param[1],((VarBool)V).data);
                        break;
                }
                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
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
                    default: // Here will be some code i think
                        break;
                }
                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                return Except(e);
            }
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
                string R = "", B = "";
                for (int i = 1; i < param.Count(); i++)
                {
                    //Prog.GetVal(param[i],B);
                    R += B;
                }

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
                bool check = false;
                //Prog.GetVal(param[0],check);

                if (check)
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