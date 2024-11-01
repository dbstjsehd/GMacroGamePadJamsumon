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
    public partial class AddTime : Form
    {
        public AddTime()
        {
            InitializeComponent();
            label5.Text = "\r\n";
            numericUpDown1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(numericUpDown1.Value + numericUpDown2.Value == 0)
            {
                button1.Text = "제일 아래\r\n에러 확인";
                label5.Text = "0ms 만큼 기다릴 수 없습니다.\r\n(왼쪽 시간과 오른쪽 시간 둘 다 0일 수 없습니다.)\r\n";
                return;
            }
            Form1.lastKey = numericUpDown1.Value + "~" + numericUpDown2.Value + "ms 대기(시간 지연)";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown2.Value < numericUpDown1.Value)
            {
                numericUpDown2.Value = numericUpDown1.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if(numericUpDown2.Value < numericUpDown1.Value)
            {
                numericUpDown1.Value = numericUpDown2.Value;
            }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) {
                button1.PerformClick();
                return;
            }

            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void numericUpDown2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) {
                button1.PerformClick();
                return;
            }
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
             if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
