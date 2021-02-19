using PhotinoNET;
using System;

namespace HelloPhotino.ThreeD
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino 3D!", options => { });
            window.NavigateToLocalFile("wwwroot/index.html");

            window.Width = 2048;
            window.Height = 1080;

            window.WaitForExit();
        }
    }
}
