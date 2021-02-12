using System;
using PhotinoWindow = PhotinoNET.PhotinoNET;

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
