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
    public partial class Form_DPAD : Form
    {
        public Form_DPAD()
        {
            InitializeComponent();
        }

        private void D_PadClick(object sender, EventArgs e)
        {
            var temp = (Button)sender;
            label2.Text = temp.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (label2.Text.Contains("No"))
            {
                this.Close();
                return;
            }

            Form1.lastKey = label2.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }
    }
}
