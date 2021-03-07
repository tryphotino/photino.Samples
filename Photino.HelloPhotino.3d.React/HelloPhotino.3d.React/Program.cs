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

            //navigate up the folders into the build folder for the 3d React App
            window.NavigateToLocalFile("../../../../../HelloPhotino.Advanced.3d.Web/build/index.html");
            window.WaitForExit();
        }
    }
}
