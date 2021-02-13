using PhotinoNET;
using System;

namespace HelloPhotino
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino Ppp", options => { });

            window.OnWebMessageReceived += (sender, message) =>
            {
                window.SendMessage("Got message: " + message);
            };

            window.NavigateToLocalFile("wwwroot/index.html");
            
            window.WaitForExit();
        }
    }
}
