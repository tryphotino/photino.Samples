using PhotinoNET;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HelloPhotinoApp
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            await Task.Run(() =>
            {
                var window = new PhotinoNET.PhotinoWindow("HelloWorldReact App");
                window.Center().LoadRawString("<h1>Hello from C#</h1>");
                window.WaitForClose();
            });
        }
    }
}
