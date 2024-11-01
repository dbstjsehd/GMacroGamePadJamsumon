using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMacroGamePadJamsumon
{
    public partial class Form_hot : Form
    {
        private int startKeyValue = -1;
        private int endKeyValue = -1;

        public Form_hot()
        {
            {
                startKeyValue = Form1.HotK[0];
                endKeyValue = Form1.HotK[1];
            }
            InitializeComponent();

            textBox1.Text = ((Keys)Form1.HotK[0]).ToString();
            textBox2.Text = ((Keys)Form1.HotK[1]).ToString();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Text = e.KeyData.ToString();
            startKeyValue = e.KeyValue;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            textBox2.Text = e.KeyData.ToString();
            endKeyValue = e.KeyValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Form1.lastKey = startKeyValue + "," + endKeyValue;
            this.Close();
        }
    }
}
