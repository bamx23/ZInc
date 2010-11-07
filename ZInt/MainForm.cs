﻿using System;
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

            Runing Run = new Runing(Cons.stdIO);
            Run.ProcMess += new ProcessMessages(ProcMess);
            Thread T = new Thread(Run.Test);
            Cons.T = T;
            T.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.Cons.Visible = !Program.Cons.Visible;
        }

    }

}