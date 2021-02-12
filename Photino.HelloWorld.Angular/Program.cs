using System;
using PhotinoWindow = PhotinoNET.PhotinoNET;

namespace HelloPhotinoAngular
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino Angular App", options => { });

            window.NavigateToLocalFile("wwwroot/index.html");
         
            window.WaitForExit();
        }
    }
}
