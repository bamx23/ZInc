using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test_OverrideCall
{
    public class SomeCl
    {
        public virtual void Do()
        {
            Console.WriteLine("No((");
        }
    }

    public class SomeClC : SomeCl
    {
        public override void Do()
        {
            Console.WriteLine("Yes");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SomeClC S = new SomeClC();
            SomeCl N = S;
            N.Do();
        }
    }
}
