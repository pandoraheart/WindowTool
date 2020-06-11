using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        ForDotNet fdn;
        bool isStart = false;
        List<IntPtr> list = new List<IntPtr>();

        public Form1()
        {
            InitializeComponent();
        }

        public Win32Api.POINT GetCursorPos()
        {
            Win32Api.POINT p;
            if (Win32Api.GetCursorPos(out p))
            {
                return p;
            }
            throw new Exception();
        }

        public IntPtr WindowFromPoint(int px, int py)
        {
            //Win32Api.POINT p = GetCursorPos();
            //return Win32Api.WindowFromPoint(p);
            return Win32Api.WindowFromPoint(px, py);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 安装鼠标钩子
            fdn = new ForDotNet();
            //fdn.SetHook();
            //fdn.MouseClickEvent += fdn_MouseClickEvent;
            #endregion

            initComboBox();
        }

        private void fdn_MouseClickEvent(object sender, MouseEventArgs e)
        {
            IntPtr hwnd = Win32Api.GetForegroundWindow();
            this.label3.Text = hwnd.ToString() + " - " ;
            //if (isStart)
            //{

            //    IntPtr hwnd = Win32Api.GetForegroundWindow();
            //    IntPtr hwnd2 = Win32Api.FindWindow(null, "Form1");
            //    this.label1.Text = hwnd + " / " + hwnd2;
            //    if (hwnd != hwnd2)
            //    {
            //        int length = Win32Api.GetWindowTextLength(hwnd);
            //        StringBuilder windowName = new StringBuilder(length + 1);
            //        Win32Api.GetWindowText(hwnd, windowName, windowName.Capacity);
            //        Win32Api.SetWindowPos(hwnd.ToInt32(), -1, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
            //        fdn.ChangeOpi(hwnd, this.trackBar1.Value);
            //        if (!list.Contains(hwnd))
            //        {
            //            list.Add(hwnd);
            //        }
            //        isStart = false;
            //    }
            //}
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (IntPtr hwnd in list)
            {
                Win32Api.SetWindowPos(hwnd.ToInt32(), -2, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                fdn.ResetOpi(hwnd);
            }
            #region 卸载鼠标钩子
            //fdn.UnHook(); 
            #endregion

            if (t != null)
            {
                stop = true;
                //
            }
        }

        private void initComboBox()
        {

            #region GetDesktopWindow获取桌面句柄，遍历子窗口
            //List<string> list = new List<string>();
            //IntPtr hwnd = Win32Api.GetDesktopWindow();
            //IntPtr childHwnd = Win32Api.GetWindow(hwnd, 5);
            //while (childHwnd != IntPtr.Zero)
            //{
            //    int length = Win32Api.GetWindowTextLength(childHwnd);
            //    StringBuilder windowName = new StringBuilder(length + 1);
            //    Win32Api.GetWindowText(childHwnd, windowName, windowName.Capacity);
            //    this.comboBox1.Items.Add(windowName.ToString());
            //    list.Add(windowName.ToString());
            //    childHwnd = Win32Api.GetWindow(childHwnd, 2);
            //}
            //Console.Write(list.ToString());

            #endregion


            #region EnumWindows获取所有顶级窗口句柄
            //WindowsFormsApplication.Win32Api.CallBack myCallBack = new WindowsFormsApplication.Win32Api.CallBack(AddItem);
            //Win32Api.EnumWindows(myCallBack, 0);  
            #endregion


            Process[] ps = Process.GetProcesses();
            ps = ps.OrderBy(p => p.ProcessName).ToArray();
            foreach (Process p in ps)
            {
                if (p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.Length > 0)
                    //if (p.MainWindowHandle != IntPtr.Zero )
                    //if (p.MainWindowHandle != IntPtr.Zero)
                    {
                    //ComboBoxItem cbi = new ComboBoxItem(p.MainWindowTitle, p.MainWindowHandle);
                    //p.ProcessName
                    ComboBoxItem cbi = new ComboBoxItem(p.MainWindowTitle, p.MainWindowHandle);
                    this.comboBox1.Items.Add(cbi);
                    //StringBuilder lpClassName = new StringBuilder(128);
                    //Win32Api.GetClassName(p.MainWindowHandle, lpClassName, lpClassName.Capacity);
                    //this.label1.Text += lpClassName.ToString() + "\r\n";
                }
            }
            if (this.comboBox1.Items.Count > 0)
                this.comboBox1.SelectedIndex = 0;
        }

        private bool AddItem(IntPtr hwnd, int lParam)
        {
            Console.WriteLine(hwnd.ToString());
            //List<string> list = new List<string>();  
            int length = Win32Api.GetWindowTextLength(hwnd);
            StringBuilder windowName = new StringBuilder(length + 1);
            Win32Api.GetWindowText(hwnd, windowName, windowName.Capacity);
            this.comboBox1.Items.Add(windowName.ToString());
            //list.Add(windowName.ToString());
            return true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItem cbi = this.comboBox1.SelectedItem as ComboBoxItem;
            if (cbi != null)
            {
                this.label1.Text = cbi.ItemValue.ToString() + " - " + cbi.ItemText;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //foreach (IntPtr hwnd in list)
            //{
            //    Win32Api.SetWindowPos(hwnd.ToInt32(), -2, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
            //    fdn.ResetOpi(hwnd);
            //}

            if (this.comboBox1.Items.Count > 0)
            {
                ComboBoxItem cbi = this.comboBox1.SelectedItem as ComboBoxItem;
                if (cbi != null)
                {
                    IntPtr hwnd = cbi.ItemValue;
                    Win32Api.SetWindowPos(hwnd.ToInt32(), -2, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                    fdn.ResetOpi(hwnd);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (!isStart)
            //    isStart = true;
            if (this.comboBox1.Items.Count > 0)
            {
                ComboBoxItem cbi = this.comboBox1.SelectedItem as ComboBoxItem;
                if (cbi != null)
                {
                    IntPtr hwnd = cbi.ItemValue;
                    Win32Api.SetWindowPos(hwnd.ToInt32(), -1, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                    //fdn.ChangeOpi(hwnd, this.trackBar1.Value);
                    if (!list.Contains(hwnd))
                    {
                        list.Add(hwnd);
                    }
                }
            }
        }
        /// <summary>
        /// Opacity Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.Items.Count > 0)
            {
                ComboBoxItem cbi = this.comboBox1.SelectedItem as ComboBoxItem;
                if (cbi != null)
                {
                    IntPtr hwnd = cbi.ItemValue;
                    //Win32Api.SetWindowPos(hwnd.ToInt32(), -1, 0, 0, 0, 0, 0x001 | 0x002 | 0x040);
                    v = this.trackBar1.Value;
                    if (!checkBox1.Checked)
                    {
                        fdn.ChangeOpi(hwnd, this.trackBar1.Value);
                    }
                    else
                    {
                        h = hwnd;
                        if (t == null)
                        {
                            t = new Thread(change);
                            t.Start();
                        }
                    }

                    if (!list.Contains(hwnd))
                    {
                        list.Add(hwnd);
                    }
                }
            }
        }

        private IntPtr h;
        private int v;
        private bool stop = false;

        Thread t;

        private void change() {
            while (true) {
                fdn.ChangeOpi(h, v);
                Thread.Sleep(100);
                if (stop)
                {
                    break;
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            v = this.trackBar1.Value;
        }
    }
}
