using PhotinoNET;
using System;

namespace HelloPhotino.Advanced.ThreeD
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("PHOTINO 3D SAMPLE APP", options =>
            {

            });

            window.RegisterWebMessageReceivedHandler((sender, message) =>
            {
                window.SendWebMessage("Got message: " + message);
            });
            window.Width = 4000;
            window.Height = 3000;

            //navigate up the folders into the build folder for the 3d React App
            window.Load("wwwroot/index.html");
            window.WaitForClose();
        }
    }
}
