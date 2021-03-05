using PhotinoNET;
using System;

namespace HelloPhotino.ThreeD
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("PONGINO", options => { });
            window.NavigateToLocalFile("wwwroot/index.html");

            window.Width = 1780;
            window.Height = 1385;

            window.WaitForExit();
        }
    }
}
