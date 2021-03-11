using PhotinoNET;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace HelloPhotinoApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string windowTitle = "Photino for .NET Demo App";

            var window = new PhotinoWindow(windowTitle)
                .Resize(800, 600)
                .Center()
                .RegisterWebMessageReceivedHandler((object sender, string message) => {
                    var window = (PhotinoWindow)sender;

                    string response = $"Received message: \"{message}\"";

                    window.OpenAlertWindow("ALert!", "Closing this crashed app!");

                    window.SendWebMessage(response);
                })
                .Load("wwwroot/index.html");
                

            window.WaitForClose();
        }
    }
}
