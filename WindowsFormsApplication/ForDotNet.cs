using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WindowsFormsApplication
{
    public class ForDotNet
    {
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int GW_CHILD = 5;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 2; 　 

        private int hHook;
        public Win32Api.HookProc hProc;

        private Point point;
        private Point Point
        {
            get { return point; }
            set
            {
                if (point != value)
                {
                    point = value;
                    if (MouseMoveEvent != null)
                    {
                        var e = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
                        MouseMoveEvent(this, e);
                    }
                }
            }
        }

        public ForDotNet()
        {
            this.Point = new Point(); 
        }

        public int SetHook()
        {
            hProc = new Win32Api.HookProc(MouseHookProc);
            hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
            if (nCode < 0)
            {
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                if (MouseClickEvent != null)
                {
                    MouseButtons button = MouseButtons.None;
                    int clickCount = 0;
                    switch ((Int32)wParam)
                    {
                        case WM_LBUTTONUP:
                            button = MouseButtons.Left;
                            clickCount = 1;
                            var e = new MouseEventArgs(button, clickCount, point.X, point.Y, 0);
                            MouseClickEvent(this, e);
                            break;
                    }

                }
                this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        public void ChangeOpi(IntPtr hwnd, int a)
        {
            int nRet = Win32Api.GetWindowLong(hwnd, GWL_EXSTYLE);
            //nRet = nRet | WS_EX_TRANSPARENT | WS_EX_LAYERED;
            nRet = nRet | WS_EX_LAYERED;
            Win32Api.SetWindowLong(hwnd, GWL_EXSTYLE, nRet);
            Win32Api.SetLayeredWindowAttributes(hwnd, 0, (byte)a, LWA_ALPHA);
        }
        public void ResetOpi(IntPtr hwnd)
        {
            int nRet = Win32Api.GetWindowLong(hwnd, GWL_EXSTYLE);
            //nRet = nRet | WS_EX_TRANSPARENT | WS_EX_LAYERED;
            nRet = nRet | WS_EX_LAYERED;
            Win32Api.SetWindowLong(hwnd, GWL_EXSTYLE, nRet);
            Win32Api.SetLayeredWindowAttributes(hwnd, 0, 255, LWA_ALPHA);
        }

        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler MouseClickEvent;


    }

    public class ComboBoxItem
    {
        private string itemText;

        public string ItemText
        {
            get { return itemText; }
            set { itemText = value; }
        }
        private IntPtr itemValue;

        public IntPtr ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }

        public ComboBoxItem() { }
        public ComboBoxItem(string t, IntPtr v)
        {
            this.itemText = t;
            this.itemValue = v;
        }

        public override string ToString()
        {
            return this.itemText;
        }
    }
}
