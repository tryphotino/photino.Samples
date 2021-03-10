using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PhotinoNET;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace HelloWorld.AdvancedNET
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().RunAsync();

            string windowTitle = "Hello Photino for .NET Advanced!";

            Action<PhotinoWindowOptions> windowConfiguration = options =>
            {
                //options.CustomSchemeHandlers.Add("app", (string url, out string contentType) =>
                //{
                //    contentType = "text/javascript";
                //    return new MemoryStream(Encoding.UTF8.GetBytes(@"
                //        (() =>{
                //            window.setTimeout(() => {
                //                alert(`🎉 Dynamically inserted JavaScript.`);
                //            }, 1000);
                //        })();
                //    "));
                //});

                options.WindowCreatingHandler += (object sender, EventArgs args) =>
                {
                    var window = (PhotinoWindow)sender;
                    Console.WriteLine($"Creating new PhotinoWindow instance.");
                };

                options.WindowCreatedHandler += (object sender, EventArgs args) =>
                {
                    var window = (PhotinoWindow)sender;
                    Console.WriteLine($"Created new PhotinoWindow instance with title {window.Title}.");
                };
            };

            var window = new PhotinoWindow(windowTitle, windowConfiguration)
                .RegisterWebMessageReceivedHandler((object sender, string message) => {
                    var window = (PhotinoWindow)sender;
                    string response = $"Received message: \"{message}\"";
                    window.SendWebMessage(response);
                });

            Size windowSize = new Size(800, 650);
            Size workAreaSize = window.MainMonitor.WorkArea.Size;

            Point centeredPosition = new Point(
                ((workAreaSize.Width / 2) - (windowSize.Width / 2)),
                ((workAreaSize.Height / 2) - (windowSize.Height / 2))
            );

            window
                .Resize(windowSize)
                .MoveTo(centeredPosition)
                .Load("wwwroot/index.html");

            window.WaitForClose();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                //webBuilder.ConfigureLogging(builder =>
                //{
                //    builder.
                //});

                //webBuilder.ConfigureKestrel(options =>
                //{
                //    options.
                //});

                webBuilder.UseStartup<Startup>();
            });
  
    }
}
