using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using MyDll;

namespace Runner
{
    class Program
    { 
        static void Main(string[] args)
        {
            ClassX Cl = new ClassX();
            Cl.Add(3, 2);
            Cl.Var = 0;
            Console.WriteLine(Cl.Add(3, 2));
        }
    }
}
