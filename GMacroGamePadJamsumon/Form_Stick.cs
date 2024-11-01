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

namespace GMacroGamePadJamsumon
{
    public partial class Form_Stick : Form
    {
        Rectangle ROI_Random;

        int leftX = -1,leftY= -1,rightX= -1,rightY= -1;

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if(leftX < 0 && rightX < 0)
                {
                    label5.Text = "Error : 그림에서 한개라도 선택해주세요.";
                    return;
                }

                this.DialogResult = DialogResult.OK;

                if(leftX > 0 && rightX > 0)
                {
                    Form1.lastKey = this.Text.Replace("설정","") + "이동(" + ROI_Random.Left+","+ROI_Random.Top+"~"+(ROI_Random.Left+ROI_Random.Width)+","+(ROI_Random.Top + ROI_Random.Height)+")";
                    this.Close();
                    return;
                }
                if(leftX > 0)
                {
                    Form1.lastKey = this.Text.Replace("설정","") + "이동(" +leftX+","+leftY +")";
                    this.Close();
                    return;
                }
                
                Form1.lastKey = this.Text.Replace("설정","") + "이동(" +rightX+","+rightY +")";
                this.Close();
                return;
                
            }

            this.DialogResult = DialogResult.OK;

            if (radioButton5.Checked)
            {
                Form1.lastKey = this.Text.Replace("설정","") + "중립(가운데로)";
                this.Close();
                return;
            }

            if (radioButton2.Checked)
            {
                Form1.lastKey = this.Text.Replace("설정","") + "누르기(Press)";
                this.Close();
                return;
            }

            if (radioButton3.Checked)
            {
                Form1.lastKey = this.Text.Replace("설정","") + "떼기(Release)";
                this.Close();
                return;
            }

            if (radioButton4.Checked)
            {
                Form1.lastKey = this.Text.Replace("설정","") + "랜덤 이동";
                this.Close();
                return;
            }
            


            

        }

        public Form_Stick()
        {
            InitializeComponent();
            this.label5.Text = "";
            Bitmap bmp = new Bitmap(254,254);
            Graphics g = Graphics.FromImage(bmp);
            Brush brush = new SolidBrush(Color.DarkGray);

            Pen pen = new Pen(Color.Black, 10);
            g.FillEllipse(brush,0,0,254,254);
            g.DrawEllipse(pen,5,5,244,244);

            
            
            pictureBox1.Image = new Bitmap(bmp);

            pen.Dispose();
            g.Dispose();
            brush.Dispose();
            bmp.Dispose();

            this.Text = Form1.lastKey + "설정";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Dispose();

            var tmpArgs = (MouseEventArgs)e;

            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;

            Bitmap bmp = new Bitmap(254,254);
            Graphics g = Graphics.FromImage(bmp);
            Brush brush = new SolidBrush(Color.DarkGray);

            Pen pen = new Pen(Color.Black, 10);
            g.FillEllipse(brush,0,0,254,254);
            g.DrawEllipse(pen,5,5,244,244);



            brush.Dispose();

            

            if(tmpArgs.Button == MouseButtons.Left)
            {
                brush = new SolidBrush(Color.Red);
                leftX = me.X;
                leftY = me.Y;

                try
                {
                    g.FillEllipse(brush,me.X -6,me.Y-6,12,12);
                }
                catch { }

                if(rightX > -1)
                {
                    brush.Dispose();
                    brush = new SolidBrush(Color.Blue);

                    try
                    {
                        g.FillEllipse(brush,rightX -6,rightY-6,12,12);
                    }
                    catch { }
                    int lx,ly,width,height;
                    if(leftX > rightX)
                    {
                        lx = rightX;

                        width = leftX - rightX;
                    }
                    else
                    {
                        lx = leftX;

                        width = rightX - leftX;
                    }

                    if(leftY > rightY)
                    {
                        ly = rightY;
                        height = leftY - rightY;
                    }
                    else
                    {
                        ly = leftY;
                        height = rightY - leftY;
                    }
                    pen.Dispose();
                    pen = new Pen(Color.Lime, 3);
                    ROI_Random = new Rectangle(lx,ly,width,height);
                    g.DrawRectangle(pen,ROI_Random);

                }
                
            }
            else if(tmpArgs.Button == MouseButtons.Right)
            {
                brush = new SolidBrush(Color.Blue);
                rightX = me.X;
                rightY = me.Y;

                try
                {
                    g.FillEllipse(brush,me.X -6,me.Y-6,12,12);
                }
                catch { }

                if(leftX > -1)
                {
                    brush.Dispose();
                    brush = new SolidBrush(Color.Red);

                    try
                    {
                        g.FillEllipse(brush,leftX -6,leftY-6,12,12);
                    }
                    catch { }

                    int lx,ly,width,height;
                    if(leftX > rightX)
                    {
                        lx = rightX;

                        width = leftX - rightX;
                    }
                    else
                    {
                        lx = leftX;

                        width = rightX - leftX;
                    }

                    if(leftY > rightY)
                    {
                        ly = rightY;
                        height = leftY - rightY;
                    }
                    else
                    {
                        ly = leftY;
                        height = rightY - leftY;
                    }
                    pen.Dispose();
                    pen = new Pen(Color.Lime, 3);
                    ROI_Random = new Rectangle(lx,ly,width,height);
                    g.DrawRectangle(pen,ROI_Random);

                }
            }

            
            
            

            pictureBox1.Image = new Bitmap(bmp);

            pictureBox1.Refresh();
            pen.Dispose();
            g.Dispose();
            brush.Dispose();
            bmp.Dispose();
            

        }
    }
}
