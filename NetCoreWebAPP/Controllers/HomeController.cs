using Microsoft.AspNetCore.Mvc;
using NetCoreWebAPP.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace NetCoreWebAPP.Controllers
{
    public class HomeController : Controller
    {

        public const int WM_CLOSE = 0x10;
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);


        public IActionResult Index()
        {
            var ps  = Process.GetProcesses().Select(s => s.ProcessName).ToList();

            return View(ps);
        }

        public IActionResult Exit()
        {
            SendMessage(StatusModel.hWnd, WM_CLOSE, 0, 0);

            Environment.Exit(0);
            return View();
        }

    }
}
