using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ListMany
{
    class Ba
    {
        public int X;
        public string S = "FUCK";

        public Ba(Ba B)
        {
            // TODO: Complete member initialization
            this.X = B.X;
        }

        public Ba()
        {
            // TODO: Complete member initialization
        }
    }
    class Program
    {
      
        static void Main(string[] args)
        {
            List<Ba> BA = new List<Ba>();
            Ba B = new Ba();
            B.X = 1000;
            for (int i = 0; i < 1000000; i++)
            {
                BA.Add(new Ba(B));
            }
            for (int i = 0; i < 1000000; i++)
            {
                BA[i].X++;
            }
            B.X = 10;
            Console.WriteLine(" ");
        }
    }
}
