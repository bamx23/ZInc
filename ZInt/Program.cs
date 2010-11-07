using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PreComp;
using Runner;
using System.Threading;


namespace ZInt
{
    public class StdIO : IO
    {
        private RichTextBox Output;

        public override void Out(string s)
        {
            lock (Output)
            {
                Output.Text += "<<" + s + "\n";
                Output.Refresh();
            }
        }

        //--------------------------------

        private TextBox Input;
        public bool WaitFor = false;

        public override string In()
        {
            string r = "";
            lock (Input)
            {
                WaitFor = true;
                Input.Focus();

                while (WaitFor)
                {
                    Application.DoEvents();
                }

                r = Input.Text;
                Input.Text = "";
            }
            return r;
        }

        public void KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Input.Enabled = false;
                Output.Text += ">>" + Input.Text + "\n";
                
                WaitFor = false;
            }
        }

        //--------------------------------
        private Thread T;
        static object locker = new object();

        public StdIO(TextBox In, RichTextBox Out)
        {
            this.Input = In;
            this.Output = Out;
            In.KeyPress += this.KeyPress;
        }

        public StdIO(TextBox In, RichTextBox Out, Thread T)
        {
            this.Input = In;
            this.Output = Out;
            In.KeyPress += this.KeyPress;
            this.T = T;
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static public Runing Run;
        static public PreCompil Pre;

        static public StdIO stdIO;

        static public Console Cons;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FMain());
        }
    }
}
