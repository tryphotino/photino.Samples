using PhotinoNET;
using System;

namespace HelloPhotino.NET
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino .NET!", options => { });

            window.RegisterWebMessageReceivedHandler((sender, message) =>
            {
                window.SendWebMessage("Got message: " + message);
            });

            window.Load("wwwroot/index.html");
            
            window.WaitForClose();
        }
    }
}
