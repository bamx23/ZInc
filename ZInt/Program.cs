using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PreComp;
using Runner;


namespace ZInt
{
    public class StdIO : IO
    {
        private RichTextBox Output;

        public override void Out(string s)
        {
             Output.Text += "<<" + s + "\n";
             Output.Refresh();
        }

        //--------------------------------

        private TextBox Input;
        public bool WaitFor = false;

        public override string In()
        {
            string r = "";
                WaitFor = true;
                Input.Focus();

                while (WaitFor)
                {
                    Application.DoEvents();
                }

                r = Input.Text;
                Input.Text = "";
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

        public StdIO(TextBox In, RichTextBox Out)
        {
            this.Input = In;
            this.Output = Out;
            In.KeyPress += this.KeyPress;
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FMain());
        }
    }
}
