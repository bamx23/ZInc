using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ZInt
{
    public partial class FMain : Form
    {
        public FMain()
        {
            InitializeComponent();
            Program.Cons = new Console();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.Cons.Visible = true;
            Program.Run = new Runner.Runing(Program.stdIO);
            Program.Run.Test();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.Cons.Visible = !Program.Cons.Visible;
        }

    }

}
