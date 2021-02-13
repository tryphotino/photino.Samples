using PhotinoNET;
using System;

namespace HelloPhotinoVue
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino Vue App", options => { });

            window.NavigateToLocalFile("wwwroot/index.html");

            window.WaitForExit();
        }
    }
}
