using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Runner
{
    public class ExprObject
    {
        public List<VarObject> param;

        public string Err;

        public virtual int Do(ref int Line)
        {
            Line++;
            return 0;
        }

        public ExprObject()
        {
        }
    }

    public class FuncCalc : ExprObject
    {
        public VarObject result;
        public override int  Do(ref int Line)
        {
            try
            {
                if (result.Type == 1)
                {
                    ((VarString)result).data = "";
                    foreach (VarObject V in param)
                        ((VarString)result).data += V.ToStr();

                    ;
                    return base.Do(ref Line);
                }

                Stack<VarObject> S = new Stack<VarObject>();
                foreach (VarObject V in param)
                {

                }
                //Тут, например, польская запись.

                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                Err = e.Message;
                return 1;
            }
        }
    }

    public class FuncIGo : ExprObject
    {
        public int thn, els;

        public override int Do(ref int Line)
        {
            try
            {

                bool R = true;
                foreach (VarObject V in param)
                {
                    //R = R && V.ToBool();
                    //Польская запись для булена :)
                }

                if (R)
                    Line = thn;
                else
                    Line = els;

                return base.Do(ref Line);
            }
            catch (Exception e)
            {
                Err = e.Message;
                return 1;
            }
        }
    }

    public class FuncIn : ExprObject
    {
        public override int Do(ref int Line)
        {
            return base.Do(ref Line);
        }
    }

    public class FuncOut : ExprObject
    {
        public override int Do(ref int Line)
        {
            return base.Do(ref Line);
        }
    }
}