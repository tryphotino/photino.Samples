
using PhotinoNET;
using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

namespace HelloPhotino.GRpc
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var builder = WebApplication
                .CreateBuilder(new WebApplicationOptions()
                {
                    WebRootPath = "wwwroot"
                });

            int port = 8080;
            int portRange = 10;
            // While port is not available choose different port
            while (IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Any(x => x.Port == port))
            {
                if (port > port + portRange)
                {
                    throw new SystemException($"Couldn't find open port within range {port - portRange} - {port}.");
                }

                port++;
            }

            builder.WebHost
                .UseUrls($"http://localhost:{port}");

            var app = builder.Build();
            app.UseStaticFiles();
            app.RunAsync();

            // Window title declared here for visibility
            string windowTitle = "Photino StaticFileServer";

            // Creating a new PhotinoWindow instance with the fluent API
            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                // Resize to a percentage of the main monitor work area
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(800, 400))
                // Center window in the middle of the screen
                .Center()
                // Users can resize windows by default.
                // Let's make this one fixed instead.
                .SetResizable(false)
                .RegisterCustomSchemeHandler("app", (object sender, string scheme, string url, out string contentType) =>
                {
                    contentType = "text/javascript";
                    return new MemoryStream(Encoding.UTF8.GetBytes(@"
                        (() =>{
                            window.setTimeout(() => {
                                alert(`🎉 Dynamically inserted JavaScript.`);
                            }, 1000);
                        })();
                    "));
                })
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWindowOptions if preferred.
                .RegisterWebMessageReceivedHandler((object sender, string message) =>
                {
                    var window = (PhotinoWindow)sender;

                    // The message argument is coming in from sendMessage.
                    // "window.external.sendMessage(message: string)"
                    string response = $"Received message: \"{message}\"";

                    // Send a message back the to JavaScript event handler.
                    // "window.external.receiveMessage(callback: Function)"
                    window.SendWebMessage(response);
                })
                .Load($"http://localhost:{port}/index.html"); // Can be used with relative path strings or "new URI()" instance to load a website.

            window.WaitForClose();
        }
    }
}
