using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Runner
{
    public class ExprObject
    {
        public List<CodeObject> param;

        public string Err;

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

        public ExprObject()
        {
        }
    }

    class ExprCalc : ExprObject
    {
        public void Int()
        {
            int R = 0;
            Stack<int> S = new Stack<int>();
            for (int i = 1; i < param.Count(); i++)
            {
                
            }
        }

        public void Strings()
        {
            string R = "";
            for (int i = 1; i < param.Count(); i++)
            {
                R += ((VarObject)param[i]).ToStr();
            }
            ((VarString)param[0]).data = R;
        }

        public void Calc()
        {
            switch (param[0].Type)
            {
                case 1:
                case 2:
                case 3:
                    Strings();
                    return;
            }
        }

        public override int Do(ref int Line)
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