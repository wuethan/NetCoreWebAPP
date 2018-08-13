using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NetCoreWebAPP.Models;
using System;
using System.Runtime.InteropServices;

namespace NetCoreWebAPP
{
    public struct WindowRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    public class Program
    {
        public static int WS_EX_LAYERED = 0x80000;
        public static int WS_EX_TRANSPARENT = 0x20;
        public static int WS_EX_DLGMODALFRAME = 0x0001;

        public static int GWL_STYLE = (-16);
        public static int GWL_EXSTYLE = (-20);
        public static int LWA_ALPHA = 0;

        public const int HWND_TOP = 0;
        public const int HWND_BOTTOM = 1;
        public const int HWND_TOPMOST = -1;
        public const int HWND_NOTOPMOST = -2;



        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);
        
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out WindowRect lpRect);



        public static void Main(string[] args)
        {
            //获取IE实例类型 
            System.Type oType = System.Type.GetTypeFromProgID("InternetExplorer.Application");

            //创建IE实例
            object IEType = System.Activator.CreateInstance(oType);

            //获取IE的所有暴露的接口
            IEType.GetType().GetMembers();

            //隐藏菜单
            IEType.GetType().InvokeMember("menubar", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { false });

            //隐藏工具条                    
            IEType.GetType().InvokeMember("toolbar", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { false });

            //隐藏状态条         
            IEType.GetType().InvokeMember("statusBar", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { false });

            //隐藏地址栏          
            IEType.GetType().InvokeMember("addressbar", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { false });

            //IE窗体可见           
            IEType.GetType().InvokeMember("Visible", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { true });

            //全屏显示          
            //IEType.GetType().InvokeMember("FullScreen", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { true });

            //大小是否可调整       
            IEType.GetType().InvokeMember("Resizable", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { false });
            
            //宽 
            IEType.GetType().InvokeMember("Width", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { 433 });

            //高
            IEType.GetType().InvokeMember("Height", System.Reflection.BindingFlags.SetProperty, null, IEType, new object[] { 423 });

            //导航到指定URL
            IEType.GetType().InvokeMember("Navigate", System.Reflection.BindingFlags.InvokeMethod, null, IEType, new object[] { "http://localhost:5901/" });

            int h = (int)IEType.GetType().InvokeMember("HWND", System.Reflection.BindingFlags.GetProperty, null, IEType, new object[] { });
            StatusModel.hWnd = (IntPtr)h;


            WindowRect rect = new WindowRect();
            GetWindowRect(StatusModel.hWnd, out rect);
            
            SetWindowLong(StatusModel.hWnd, GWL_EXSTYLE, GetWindowLong(StatusModel.hWnd, GWL_EXSTYLE) | WS_EX_DLGMODALFRAME);

            SetWindowPos(StatusModel.hWnd, (IntPtr)HWND_TOPMOST, rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top, 0);


            //var p = System.Diagnostics.Process.GetProcessesByName("iexplore");

            //var s = GetWindowLong(p[0].MainWindowHandle, GWL_EXSTYLE) ;
            //SetWindowLong(p[0].MainWindowHandle, GWL_EXSTYLE,s | WS_EX_LAYERED);
            //SetLayeredWindowAttributes(p[0].MainWindowHandle, 0, -100, 2);


            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5901")
                .UseStartup<Startup>();
    }
}
