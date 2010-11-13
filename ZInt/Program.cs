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
            AddOut(s);
        }

        //--------------------------------

        private TextBox Input;
        public bool WaitFor = false;

        public override string In()
        {
            string r = "";
                WaitFor = true;
                InFocus();

                while (WaitFor)
                    Thread.Sleep(100);

                r = GetIn();
            return r;
        }

        public void KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                              
                WaitFor = false;
            }
        }

        //--------------------------------

        public StdIO(TextBox In, RichTextBox Out/*, Thread Cur*/)
        {
            this.Input = In;
            this.Output = Out;
            //this.Cur = Cur;
            In.KeyPress += this.KeyPress;
        }

        //--------------------------------

        private delegate bool BoolDelegate();

        private bool InFocusDo()
        {
            Input.Enabled = true;
            Input.Focus();
            return true;
        }

        private void InFocus()
        {
            if (Input.InvokeRequired)
            {
                Input.Invoke(new BoolDelegate(InFocusDo));
            }
            else
            {
                InFocusDo();
            }
        }

        //--------------------------------

        private delegate string GetInputTextDelegate();

        private string GetInputText()
        {
            string strText = Input.Text;
            Input.Enabled = false;
            Output.Text += ">>" + Input.Text + "\n";
            Input.Text = "";
            return strText;
        }

        private string GetIn()
        {
            string strText;
            if (Input.InvokeRequired)
            {
                GetInputTextDelegate GTD = new GetInputTextDelegate(GetInputText);
                strText = Convert.ToString(Input.Invoke(GTD));
            }
            else
            {
                strText = GetInputText();
            }
            return strText;
        }

        //--------------------------------

        private delegate void AddOutputTextDelegate(string s);

        private void AddOutText(string s)
        {

            Output.Text += "<<" + s + "\n";
            Output.Refresh();
        }

        private void AddOut(string s)
        {
            if (Output.InvokeRequired)
            {
                AddOutputTextDelegate GTD = new AddOutputTextDelegate(AddOutText);
                Output.Invoke(GTD, (object)s);
            }
            else
            {
                AddOutText(s);
            }
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
