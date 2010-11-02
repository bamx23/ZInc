using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PreComp;
using Runner;


namespace ZInt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static public Runing Run = new Runing();
        static public PreCompil Pre = new PreCompil();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FMain());
        }
    }
}
