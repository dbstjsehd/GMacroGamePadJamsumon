using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace GMacroGamePadJamsumon
{
    public partial class AddKey : Form
    {
        string fullpath;
        public AddKey()
        {
            fullpath = Application.StartupPath + "\\img\\";
            InitializeComponent();

            this.pictureBox1.BackgroundImage = new Bitmap(fullpath+"dualShock4.png");
            this.pictureBox2.BackgroundImage = new Bitmap(fullpath+"dualShock4_1.png");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;

            

            if(me.X >= 55 && me.X <= 128 && me.Y >= 55 && me.Y <= 128)
            {
                var frm = new Form_DPAD();
                DialogResult temp = frm.ShowDialog();
                if (temp == DialogResult.OK)
                {
                    textBox1.Text = "D-PAD 키 : " + Form1.lastKey;
                }

                return;
            }
            

            if(me.X >= 129 && me.X <= 202 && me.Y >= 114 && me.Y <= 180)
            {
                Form1.lastKey = "왼쪽 스틱 ";
                
                var frm = new Form_Stick();
                DialogResult temp = frm.ShowDialog();
                if (temp == DialogResult.OK)
                {
                    textBox1.Text = Form1.lastKey;
                }

                return;
            }

            if(me.X >= 281 && me.X <= 357 && me.Y >= 114 && me.Y <= 180)
            {
                Form1.lastKey = "오른쪽 스틱 ";
                
                var frm = new Form_Stick();
                DialogResult temp = frm.ShowDialog();
                if (temp == DialogResult.OK)
                {
                    textBox1.Text = Form1.lastKey;
                }

                return;
            }


            radioButton1.Checked = true;

            if(me.X >= 132 && me.X <= 155 && me.Y >= 33 && me.Y <= 58)
            {
                textBox1.Text = "Share 키";

                return;
            }

            if(me.X >= 227 && me.X <= 260 && me.Y >= 131 && me.Y <= 160)
            {
                textBox1.Text = "Ps 키";

                return;
            }

            if(me.X >= 333 && me.X <= 353 && me.Y >= 32 && me.Y <= 58)
            {
                textBox1.Text = "Option 키";

                return;
            }
            if(me.X >= 162 && me.X <= 325 && me.Y >= 26 && me.Y <= 93)
            {
                textBox1.Text = "터치패드 키";

                return;
            }
            if(me.X >= 379 && me.X <= 409 && me.Y >= 40 && me.Y <= 71)
            {
                textBox1.Text = "△ 키";

                return;
            }
            if(me.X >= 344 && me.X <= 375 && me.Y >= 72 && me.Y <= 102)
            {
                textBox1.Text = "□ 키";

                return;
            }
            if(me.X >= 375 && me.X <= 413 && me.Y >= 103 && me.Y <= 137)
            {
                textBox1.Text = "X 키";

                return;
            }
            if(me.X >= 416 && me.X <= 450 && me.Y >= 72 && me.Y <= 102)
            {
                textBox1.Text = "○ 키";

                return;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;


            

            if(me.X >= 51 && me.X <= 119 && me.Y >= 90 && me.Y <= 122)
            {
                textBox1.Text = "R1 키";

                return;
            }

            if(me.X >= 51 && me.X <= 119 && me.Y >= 123 && me.Y <= 190)
            {
                textBox1.Text = "R2 키";

                return;
            }

            if(me.X >= 363 && me.X <= 426 && me.Y >= 90 && me.Y <= 122)
            {
                textBox1.Text = "L1 키";

                return;
            }

            if(me.X >= 363 && me.X <= 426 && me.Y >= 123 && me.Y <= 190)
            {
                textBox1.Text = "L2 키";

                return;
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Contains("D-PAD") || textBox1.Text.Contains("스틱"))
            {
                groupBox1.Visible = false;
            }
            else
            {
                groupBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Contains("아무"))
            {
                return;
            }

            if (groupBox1.Visible)
            {
                if (radioButton1.Checked)
                {
                    Form1.lastKey = textBox1.Text + " 누르기(Press)";
                }
                else
                {
                    Form1.lastKey = textBox1.Text + " 떼기(Release)";
                }
            }
            else
            {
                Form1.lastKey = textBox1.Text;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
