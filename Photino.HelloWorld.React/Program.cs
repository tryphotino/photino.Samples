using System;
using PhotinoWindow = PhotinoNET.PhotinoNET;

namespace HelloPhotinoReact
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino React App", options => { });

            window.NavigateToLocalFile("wwwroot/index.html");

            window.WaitForExit();
        }
    }
}
