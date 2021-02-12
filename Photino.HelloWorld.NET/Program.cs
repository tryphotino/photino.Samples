using System;
using System.IO;
using System.Text;
using photino=PhotinoNET;

namespace HelloWorldApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new photino.PhotinoNET("My first Photino app", options => { });

            window.OnWebMessageReceived += (sender, message) =>
            {
                window.SendMessage("Got message: " + message);
            };

            window.NavigateToLocalFile("wwwroot/index.html");
            
            window.WaitForExit();
        }
    }
}
