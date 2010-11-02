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
        public int ErrCode;


        public virtual int Do(int Line)
        {
            return Line+1;
        }

        public ExprObject()
        {
        }

        public ExprObject(int LINE)
        {
            Err = ""; ErrCode = 0;
        }
    }

    public class FuncCalc : ExprObject
    {
        public VarObject result;
        public override int Do(int Line)
        {
            if (result.Type == 1)
            {
                ((VarString)result).data = "";
                foreach (VarObject V in param)
                    ((VarString)result).data += V.ToStr();
                return base.Do(Line);
            }

            Stack<VarObject> S = new Stack<VarObject>();
            foreach (VarObject V in param)
            {                

            }
            //Тут, например, польская запись.

            return base.Do(Line);
        }
    }

    public class FuncIf : ExprObject
    {
        public int thn, els;

        public override int Do(int Line)
        {
            bool R = true;
            foreach (VarObject V in param)
            {
                //R = R && V.ToBool();
                //Польская запись для булена :)
            }

            if (R)
                return thn;
            else
                return els;
        }
    }

    public class FuncIn : ExprObject
    {
        public override int Do(int Line)
        {
            return base.Do(Line);
        }
    }

    public class FuncOut : ExprObject
    {
        public override int Do(int Line)
        {
            return base.Do(Line);
        }
    }
}