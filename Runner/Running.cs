using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Runner
{
    public class Runing
    {
        public List<VarObject> Var;

        public List<ExprObject> Lines;

        public Runing()
        {
            Var = new List<VarObject>();
        }


        public string Test()
        {
            Var.Add(new VarInt(1));
            ((VarInt)Var[0][5][2]).data = 10;
            ((VarInt)Var[0][5][11]).data = 8;
            Var.Add(new VarDouble(1));
            //Var[1][5][3].ParseString(Var[0][5][11].ToStr());
            DynVarObject V = new DynVarObject("0.5.11");

            return V.Get(Var).ToStr();
        }

        public void Run()
        {
            int Cur = 0;
            bool Err = false;
            while (Cur <= Lines.Count() && (Err == false))
            {
                Err = (Lines[Cur].Do(ref Cur) != 0);
            }
            if (Err)
            {
                //Lines[Cur].Err;
            }
        }
    }
}
