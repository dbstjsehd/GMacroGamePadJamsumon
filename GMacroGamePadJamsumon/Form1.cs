using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets.DualShock4;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMacroGamePadJamsumon
{
    public partial class Form1 : Form
    {
        public static string lastKey = "";
        public static int listViewLastIndex = -1;
        public static int[] HotK = new int[2];
        bool bool_repeat = true;

        ViGEmClient client;
        Nefarius.ViGEm.Client.Targets.IDualShock4Controller ds4;
        Random rand;

        Thread thread_메인;

        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hwnd, int id);

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                switch (m.WParam.ToInt32())
                {
                    case 1:
                    case 1001:
                    case 2001:
                        {
                           componentsEnable(false);

                            if(listBox1.Items.Count == 0)
                            {
                                return;
                            }

                            if (thread_메인.ThreadState != System.Threading.ThreadState.Running)
                            {
                                try 
                                { 
                                    thread_메인.Abort();
                                } catch { }
                                thread_메인 = new Thread(new ThreadStart(main));
                                thread_메인.Start();
                            }

                        }
                        break;

                    case 2:
                    case 1002:
                    case 2002:
                        {
                            componentsEnable(true);
                            
                            try
                            {
                                thread_메인.Abort();
                            }
                            catch { }
                            ReleaseAllKey();
                        }
                        break;


                    default:
                        break;

                }
            }
        }


        public void componentsEnable(bool enabled)
        {
            this.menuStrip1.Enabled = enabled;
            radioButton1.Enabled = enabled;
            radioButton2.Enabled = enabled;
            button1.Enabled = enabled;
            button2.Enabled = enabled;
            button3.Enabled = enabled;
            button4.Enabled = enabled;
        }


        public Form1()
        {
            try{
                HotK[0] = 116;
                HotK[1] = 117;

                HotK[0] = Properties.Settings.Default.startHot;
                HotK[1] = Properties.Settings.Default.endHot;
            }catch { }
            


            {
                rand = new Random();

                try { client = new ViGEmClient(); } catch { }
                try { ds4 = client.CreateDualShock4Controller(); } catch { }
                try
                {
                    ds4.Connect();
                }
                catch { }
                
                try
                {
                    ds4.AutoSubmitReport = false;
                }
                catch { }
                try
                {
                    ds4.ResetReport();
                }
                catch { }
                try
                {
                    ds4.Disconnect();

                }
                catch { }
                try
                {
                    ds4.Dispose();

                }
                catch { }
                try
                {
                    client.Dispose();

                }
                catch { }
            }

            client = new ViGEmClient();
            ds4 = client.CreateDualShock4Controller();

            ds4.Connect();
            ds4.ResetReport();
            ds4.AutoSubmitReport = true;



            InitializeComponent();

            thread_메인 = new Thread(new ThreadStart(main));
            thread_메인.Abort();

            {
                string[] strings = new string[2];
                
                for(int i = 0 ; i < 2; i++)
                {
                    
                    
                    strings[i] = ((Keys)HotK[i]).ToString();
                    
                }
                


                label1.Text = "시작 : " + strings[0] + "\r\n종료 : " + strings[1];

                RegisterHotKey(this.Handle, 1, 0, HotK[0]);
                RegisterHotKey(this.Handle, 1001, 2, HotK[0]);
                RegisterHotKey(this.Handle, 2001, 4, HotK[0]);

                RegisterHotKey(this.Handle, 2, 0, HotK[1]);
                RegisterHotKey(this.Handle, 1002, 2, HotK[1]);
                RegisterHotKey(this.Handle, 2002, 4, HotK[1]);


            }

        }

        private Action stringToAction(string value)
        {
            if(value.Contains("스틱"))
            {
                var tempStickX = DualShock4Axis.LeftThumbX;
                var tempStickY = DualShock4Axis.LeftThumbY;
                var tempStick = DualShock4Button.ThumbLeft;

                if (value.Contains("오른쪽"))
                {
                    tempStickX = DualShock4Axis.RightThumbX;
                    tempStickY = DualShock4Axis.RightThumbY;
                    tempStick = DualShock4Button.ThumbRight;
                }

                if(value.Contains("랜덤 이동"))
                {
                    return () => {
                        ds4.SetAxisValue(tempStickX, (byte)rand.Next(0, 255));
                        ds4.SetAxisValue(tempStickY, (byte)rand.Next(0, 255));
                    };
                }

                if (value.Contains("중립"))
                {
                    return () => {
                        ds4.SetAxisValue(tempStickX, 127);
                        ds4.SetAxisValue(tempStickY, 127);
                    };
                }

                if (value.Contains("누르기"))
                {
                    return() => {
                        ds4.SetButtonState(tempStick,true);
                    };
                }
                if (value.Contains("떼기"))
                {
                    return() => {
                        ds4.SetButtonState(tempStick,false);
                    };
                }
                if (value.Contains("이동"))
                {
                    if (value.Contains("~"))
                    {
                        value = value.Replace(")","");
                        var tmp = value.Split('(');
                        var tmp2 = tmp[1].Split('~');

                        var leftPosition = tmp2[0].Split(',');
                        var rightPosition = tmp2[1].Split(',');


                        return () => {
                            ds4.SetAxisValue(tempStickX, (byte)(rand.Next(int.Parse(leftPosition[0]), int.Parse(rightPosition[0])) + 1));
                            ds4.SetAxisValue(tempStickY, (byte)(rand.Next(int.Parse(leftPosition[1]), int.Parse(rightPosition[1])) + 1));
                        };

                    }
                    else
                    {
                        value = value.Replace(")","");
                        var tmp = value.Split('(');
                        var position = tmp[1].Split(',');

                        return () => {
                            ds4.SetAxisValue(tempStickX, (byte)(int.Parse(position[0]) + 1));
                            ds4.SetAxisValue(tempStickY, (byte)(int.Parse(position[1]) + 1));
                        };

                    }
                }

            }

            if(value.Contains("시간 지연"))
            {
                try
                {
                        
                    string[] words = value.Split('~');
                    int first = int.Parse(words[0]);
                    int after = int.Parse(words[1].Split('m')[0]);
                    return () => Delay(rand.Next(first, after));
                }
                catch
                {
                    return () => Delay(rand.Next(100, 200));
                }
            }

            if(value.Contains("All Release"))
            {
                return () => ReleaseAllKey();
            }

            if (value.Contains("D-PAD"))
            {
                string tmpKey = value.Split(new string[] { " : "}, StringSplitOptions.None)[1];
                var dPadKey = DualShock4DPadDirection.None;
                if (tmpKey.Equals("↖"))
                {
                    dPadKey = DualShock4DPadDirection.Northwest;
                }
                else if (tmpKey.Equals("↑"))
                {
                    dPadKey = DualShock4DPadDirection.North;
                }
                else if (tmpKey.Equals("↗"))
                {
                    dPadKey = DualShock4DPadDirection.Northeast;
                }
                else if (tmpKey.Equals("←"))
                {
                    dPadKey = DualShock4DPadDirection.West;
                }
                else if (tmpKey.Equals("→"))
                {
                    dPadKey = DualShock4DPadDirection.East;
                }
                else if (tmpKey.Equals("↙"))
                {
                    dPadKey = DualShock4DPadDirection.Southwest;
                }
                else if (tmpKey.Equals("↓"))
                {
                    dPadKey = DualShock4DPadDirection.South;
                }
                else if (tmpKey.Equals("↘"))
                {
                    dPadKey = DualShock4DPadDirection.Southeast;
                }

                return () => ds4.SetDPadDirection(dPadKey);

            }

            if (value.Contains(" 키 "))
            {
                DualShock4Button tempButton = DualShock4SpecialButton.Touchpad;

                if (value.Contains("L2"))
                {
                    if (value.Contains("누르기"))
                    {
                        return () => ds4.SetSliderValue(DualShock4Slider.LeftTrigger, 255);
                    }
                    else
                    {
                        return () => ds4.SetSliderValue(DualShock4Slider.LeftTrigger, 0);
                    }
                }
                if (value.Contains("R2"))
                {
                    if (value.Contains("누르기"))
                    {
                        return () => ds4.SetSliderValue(DualShock4Slider.RightTrigger, 255);
                    }
                    else
                    {
                        return () => ds4.SetSliderValue(DualShock4Slider.RightTrigger, 0);
                    }
                }

                if (value.Contains("△"))
                {
                    tempButton = DualShock4Button.Triangle;
                }
                else if (value.Contains("X"))
                {
                    tempButton = DualShock4Button.Cross;
                }
                else if (value.Contains("□"))
                {
                    tempButton = DualShock4Button.Square;
                }
                else if (value.Contains("○"))
                {
                    tempButton = DualShock4Button.Circle;
                }
                else if (value.Contains("R1"))
                {
                    tempButton = DualShock4Button.ShoulderRight;
                }
                else if (value.Contains("L1"))
                {
                    tempButton = DualShock4Button.ShoulderLeft;
                }
                else if (value.Contains("Option"))
                {
                    tempButton = DualShock4Button.Options;
                }
                else if (value.Contains("Share"))
                {
                    tempButton = DualShock4Button.Share;
                }
                else if (value.Contains("Ps"))
                {
                    tempButton = DualShock4SpecialButton.Ps;
                }

                if (value.Contains("누르기"))
                {
                    return () => ds4.SetButtonState(tempButton, true);
                }
                else
                {
                    return () => ds4.SetButtonState(tempButton, false);
                }

            }


            return null;
        }


        private void ReleaseAllKey()
        {
            ds4.SetAxisValue(DualShock4Axis.LeftThumbX, 127);
            ds4.SetAxisValue(DualShock4Axis.LeftThumbY, 127);
            foreach (var temp in DualShock4Button.GetAll<DualShock4Button>())
            {
                ds4.SetButtonState(temp, false);
            }
            foreach(var temp in DualShock4SpecialButton.GetAll<DualShock4SpecialButton>())
            {
                ds4.SetButtonState(temp, false);
            }

            ds4.SetDPadDirection(DualShock4DPadDirection.None);
            ds4.SetSliderValue(DualShock4Slider.LeftTrigger, 0);
            ds4.SetSliderValue(DualShock4Slider.RightTrigger, 0);
            ds4.SetAxisValue(DualShock4Axis.RightThumbY, 127);
            ds4.SetAxisValue(DualShock4Axis.RightThumbX, 127);
        }


        private void about뜻길잠수몬ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_About about = new Form_About();
            about.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new AddKey();
            DialogResult temp = frm.ShowDialog();
            if (temp == DialogResult.OK)
            {
                if (radioButton1.Checked)
                {
                    listBox1.Items.Add(Form1.lastKey);
                }
                else
                {
                    try
                    {
                        listBox1.Items.Insert(listViewLastIndex, Form1.lastKey);
                    }
                    catch
                    {
                        listBox1.Items.Add(Form1.lastKey);
                    }
                }
            }
        }

        private void main()
        {
            int Count = listBox1.Items.Count;
            Action[] ActionArray = new Action[Count];

            for(int i = 0 ; i < Count ; i++)
            {
                string str = string.Format("{0} ", listBox1.Items[i]);
                ActionArray[i] = stringToAction(str);
            }

            do
            {
                for(int i = 0 ; i < Count ; i++)
                {
                    this.Invoke(new MethodInvoker(delegate() 
                    { 
                        listBox1.SelectedIndex = i; 
                    }));
                    ActionArray[i]();
                }
            }while(bool_repeat);
            this.Invoke(new MethodInvoker(delegate() 
            { 
                componentsEnable(true);
            }));
        }

        private static DateTime Delay(int ms)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, ms);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;

            }
            return DateTime.Now;
        }       // thread sleep 대체

        private void button2_Click(object sender, EventArgs e)
        {
            var frm = new AddTime();
            DialogResult temp = frm.ShowDialog();
            if (temp == DialogResult.OK)
            {
                if (radioButton1.Checked)
                {
                    listBox1.Items.Add(Form1.lastKey);
                }
                else
                {
                    try
                    {
                        listBox1.Items.Insert(listViewLastIndex, Form1.lastKey);
                    }
                    catch
                    {
                        listBox1.Items.Add(Form1.lastKey);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int tmp = listViewLastIndex;
                listBox1.Items.RemoveAt(tmp);
                listBox1.SelectedIndex = tmp;
            }
            catch { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewLastIndex = listBox1.SelectedIndex;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool_repeat = checkBox1.Checked;
        }

        private void 단축키설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Form_hot();
            DialogResult temp = frm.ShowDialog();
            if (temp == DialogResult.OK)
            {
                var splits = Form1.lastKey.Split(',');
                string[] strings = new string[2];
                for(int i = 0 ; i < 2 ; i++)
                {
                    HotK[i] = int.Parse(splits[i]);
                }

                for(int i = 0 ; i < 2; i++)
                {
                    
                    
                    strings[i] = ((Keys)HotK[i]).ToString();
                    
                }
                


                label1.Text = "시작 : " + strings[0] + "\r\n종료 : " + strings[1];

                Properties.Settings.Default.startHot = HotK[0];
                Properties.Settings.Default.endHot = HotK[1];
                Properties.Settings.Default.Save();


                UnregisterHotKey(this.Handle,1);
                UnregisterHotKey(this.Handle,1001);
                UnregisterHotKey(this.Handle,2001);
                UnregisterHotKey(this.Handle,2);
                UnregisterHotKey(this.Handle,1002);
                UnregisterHotKey(this.Handle,2002);

                RegisterHotKey(this.Handle, 1, 0, HotK[0]);
                RegisterHotKey(this.Handle, 1001, 2, HotK[0]);
                RegisterHotKey(this.Handle, 2001, 4, HotK[0]);

                RegisterHotKey(this.Handle, 2, 0, HotK[1]);
                RegisterHotKey(this.Handle, 1002, 2, HotK[1]);
                RegisterHotKey(this.Handle, 2002, 4, HotK[1]);
            }
        }

        private void 새마l크로ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listViewLastIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                listBox1.Items.Add("모든 키 떼기(All Release)");
            }
            else
            {
                try
                {
                    listBox1.Items.Insert(listViewLastIndex, "모든 키 떼기(All Release)");
                }
                catch
                {
                    listBox1.Items.Add("모든 키 떼기(All Release)");
                }
            }
        }

        private void 끝ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 마l크로저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listBox1.Items.Count == 0)
            {
                MessageBox.Show("저장할 내용이 없습니다.");
                return;
            }


            string path = Application.StartupPath + "\\saveData\\";
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }
            }
            

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = path;
            saveFileDialog.Filter = "SettingFiles|*.jamsuPad";
            saveFileDialog.Title = "세팅 파일 저장";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] saveLines = new string[listBox1.Items.Count];
                for(int i = 0 ; i< saveLines.Length ; i++) {
                    saveLines[i] = listBox1.Items[i].ToString();
                }

                File.WriteAllLines(saveFileDialog.FileName,saveLines);
            }



        }

        private void ㅁl크로불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath + "\\saveData\\";
                openFileDialog.Filter = "SettingFiles|*.jamsuPad";
                openFileDialog.Title = "세팅 파일 불러오기";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] Lines = File.ReadAllLines(openFileDialog.FileName);

                    listBox1.Items.Clear();
                    for(int i = 0 ; i < Lines.Length ; i++)
                    {
                        listBox1.Items.Add(Lines[i]);
                    }
                }
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {            
            try
            {
                thread_메인.Abort();
            }
            catch { }

            try
            {
                ReleaseAllKey();
            }
            catch { }
            try
            {
                ds4.AutoSubmitReport = false;
            }
            catch { }
            try
            {
                ds4.ResetReport();
            }
            catch { }
            try
            {
                ds4.Disconnect();

            }
            catch { }
            try
            {
                ds4.Dispose();

            }
            catch { }
            try
            {
                client.Dispose();

            }
            catch { }

            System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
            foreach (System.Diagnostics.Process p in mProcess)
            {
                p.Kill();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ds4.AutoSubmitReport = false;
            }
            catch { }
            try
            {
                ds4.ResetReport();
            }
            catch { }
            try
            {
                ds4.Disconnect();

            }
            catch { }
            try
            {
                ds4.Dispose();

            }
            catch { }
            try
            {
                client.Dispose();

            }
            catch { }

            client = new ViGEmClient();
            ds4 = client.CreateDualShock4Controller();

            ds4.Connect();
            ds4.ResetReport();
            ds4.AutoSubmitReport = true;
        }
    }
}
