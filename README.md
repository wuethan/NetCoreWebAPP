## Net Core 2.1  GUI 解决方案：

## 概述：
Net Core 的确很强大，它不仅可以通过本地化快速启动asp.net站点同样支持通过控制台访问系统函数，甚至是SerialPort串口开发，你可以看到
启动IE的方法中可以设置全屏，这很利于上位机的开发，通过浏览器按钮跳转访问controller执行Process访问其他程序，当然它的功能不止这么简单，目前仅支持Windows系统 ，欢迎提议。

## 结构：
利用Net Core 控制台 启动 ASP.Net 网站 + 启动IE浏览器程序并跳转至设置http://localhost:5901

## 工具：
 Visual Studio 2017

## 实现：<br/>
1. 新建**Net Core 2.1** mvc视图<br/>

2. 项目-属性-调试-取消启动浏览器。<br/>

3. Program 的 CreateWebHostBuilder 方法中加入 <br/>

```c#
.UseUrls("http://localhost:5901")
```
使它看起像
```c#
 public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:5901")
                .UseStartup<Startup>();
```

关键在于反射IE并导航的方法中端口号和地址必须相同：

```c#
//导航到指定URL
IEType.GetType().InvokeMember("Navigate", System.Reflection.BindingFlags.InvokeMethod, null, IEType, new object[] { "http://localhost:5901/" });
```

通过mvc特性访问Controller并执行如下方法关闭
```c#
public IActionResult Exit()
{
    SendMessage(StatusModel.hWnd, WM_CLOSE, 0, 0);

    Environment.Exit(0);
    return View();
}
```

导航实例目录下执行
dotnet publish -r win7-x64
运行publish目录中的exe 你会看到程序飞快的运行了GUI（浏览器）程序并获得了进程列表，通过关闭退出！
