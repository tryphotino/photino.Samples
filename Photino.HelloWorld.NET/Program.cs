using System;
using System.IO;
using System.Text;
using PhotinoWindow=PhotinoNET.PhotinoNET;

namespace HelloPhotino
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("My first Photino app", options => { });

            window.OnWebMessageReceived += (sender, message) =>
            {
                window.SendMessage("Got message: " + message);
            };

            window.NavigateToLocalFile("wwwroot/index.html");
            
            window.WaitForExit();
        }
    }
}
