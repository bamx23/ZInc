using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDll
{
    public class ClassX
    {
        public int Var = 0;
        public int Add(int a, int b)
        {
            Var++;
            return a*2 + b;
        }
    }
}
