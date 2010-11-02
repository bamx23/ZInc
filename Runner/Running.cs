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
            ((VarInt)Var[0][5][10]).data = 5;
            ((VarInt)Var[0][5][11]).data = 10;
            Var.Add(new VarDouble(1));
            //Var[1][5][3].ParseString(Var[0][5][11].ToStr());

            return Var[1][5][3].ToStr();
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
