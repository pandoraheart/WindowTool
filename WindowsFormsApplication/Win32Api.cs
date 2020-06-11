using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication
{
    public class Win32Api
    {
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate bool CallBack(IntPtr hwnd, int lParam); 

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        //从类名或窗口名中返回一个相匹配的顶层窗口的句柄
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //返回包含有指定点的窗口句柄
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);
        //返回包含有指定点的窗口句柄
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);
        //返回当前的光标位置
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        //安装钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        //卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        //调用下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        //返回指定窗口的标题栏文本的长度
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        //把指定窗口的标题栏文本拷贝到指定缓冲区中
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);
        //返回桌面窗口的句柄
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        //返回指定窗口的句柄
        [DllImport("user32.dll", EntryPoint = "GetWindow")]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);
        //检取与调用此函数的线程有关的活动窗口句
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        //返回用户当前工作用的窗口句柄
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        //返回指定窗口的附加窗口内存的地址(32位)
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);
        //修改给定窗口的一个属性，并在附加窗口内存的指定偏移处设置新值(32位)
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
        //设置透明
        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(IntPtr Handle, int crKey, byte bAlpha, int dwFlags);
        //枚举所有屏幕上的顶层窗口，并将窗口句柄传送给应用程序定义的回调函数
        [DllImport("user32")]
        public static extern int EnumWindows(CallBack x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
    }
}
