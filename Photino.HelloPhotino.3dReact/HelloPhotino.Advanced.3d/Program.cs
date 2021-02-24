using System;
using System.IO;
using System.Text;

namespace HelloPhotino.Advanced.ThreeD
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoNET.PhotinoNET("PHOTINO 3D SAMPLE APP", options =>
            {

            });

            window.OnWebMessageReceived += (sender, message) =>
            {
                window.SendMessage("Got message: " + message);
            };
            window.Width = 4000;
            window.Height = 3000;
            window.NavigateToLocalFile("wwwroot/build/index.html");
            window.WaitForExit();
        }
    }
}
