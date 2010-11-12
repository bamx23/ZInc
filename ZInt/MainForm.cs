using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PreComp;
using Runner;
using System.Threading;

namespace ZInt
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
        }

        static void ProcMess()
        {
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console Cons = new Console();
            Cons.Visible = true;

            string sCode = "#0 new int\n" +
                "#0 new int\n" +
                "#1 in std V0\n"+
                "#1 ifgo (T1 V0 <) 5 8\n"+
                "#1 in std V1.[T1]\n"+
                "#1 inc T1\n"+
                "#1 ifgo (T1 V0 <) 5 8\n"+
                "#1 set T1 0\n"+
                "#2 ifgo (T1 V0 <) 10 13\n"+
                "#2 set V1.[T1] (V1.[T1] V0 +)\n"+
                "#2 inc T1\n"+
                "#2 ifgo (T1 V0 <) 10 13\n"+
                "#2 set T1 0\n"+
                "#3 ifgo (T1 V0 <) 15 18\n"+
                "#3 out std V1.[T1]\n"+
                "#3 inc T1\n"+
                "#3 ifgo (T1 V0 <) 15 18\n"+
                "#4 return";

            Runing Run = new Runing(Cons.stdIO,sCode.Split('\n').ToList<string>());
            Run.ProcMess += new ProcessMessages(ProcMess);
            Thread T = new Thread(Run.Run);
            Cons.CurThread = T;
            T.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void FMain_Load(object sender, EventArgs e)
        {

        } 

    }

}
