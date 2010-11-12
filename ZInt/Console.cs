using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ZInt
{
    public partial class Console : Form
    {
        public StdIO stdIO;
        public Thread CurThread;

        public Console()
        {
            InitializeComponent();
            stdIO = new StdIO(textBox1, richTextBox1);
            richTextBox1.Clear();
        }

        private void haltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurThread.Abort();
            Close();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void Console_FormClosed(object sender, FormClosedEventArgs e)
        {
            CurThread.Abort();
        }

        
    }
}
