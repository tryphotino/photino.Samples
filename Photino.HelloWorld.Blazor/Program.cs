using Photino.Blazor;
using System;

namespace HelloPhotinoBlazor
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ComponentsDesktop.Run<Startup>("Hello Photino Blazor App", "wwwroot/index.html");
        }
    }
}
