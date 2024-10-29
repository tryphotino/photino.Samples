using Photino.NET;
using PhotinoNET.Server;
using System.Drawing;
using System.Text;

namespace Photino.HelloPhotino.Angular;
//NOTE: To hide the console window, go to the project properties and change the Output Type to Windows Application.
// Or edit the .cspro file and change the <OutputType> tag from "WinExe" to "Exe".

class Program
{
#if DEBUG
    public static bool IsDebugMode = true;
#else
    public static bool IsDebugMode = false;
#endif

    [STAThread]
    static void Main(string[] args)
    {
        //rename photino-hellophotino-angular to the name of your app listed in your package.json
        PhotinoServer
            .CreateStaticFileServer(args, 8000, 100, "wwwroot/photino-hellophotino-angular/browser/", out string baseUrl)
            .RunAsync();

        // The appUrl is set to the local development server when in debug mode.
        // This helps with hot reloading and debugging.
        string appUrl = IsDebugMode ? "http://localhost:4200" : $"{baseUrl}/index.html";
        Console.WriteLine($"Serving Angular app at {appUrl}");

        // Window title declared here for visibility
        string windowTitle = "Photino.Angular Demo App";

        // Creating a new PhotinoWindow instance with the fluent API
        var window = new PhotinoWindow()
            .SetTitle(windowTitle)
            .SetUseOsDefaultSize(false)
            .SetSize(new Size(2048, 1024))
            // Resize to a percentage of the main monitor work area
            //.Resize(50, 50, "%")
            .SetUseOsDefaultSize(false)
            .SetSize(new Size(800, 600))
            // Center window in the middle of the screen
            .Center()
            // Users can resize windows by default.
            // Let's make this one fixed instead.
            .SetResizable(true)
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
            .Load(appUrl); // Can be used with relative path strings or "new URI()" instance to load a website.

        window.WaitForClose(); // Starts the application event loop
    }
}