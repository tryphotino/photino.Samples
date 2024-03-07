using Microsoft.Extensions.DependencyInjection;
using Photino.NET;
using Photino.NET.Extensions;
using Photino.NET.IPC;
using Photino.NET.Options;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        // Creates the default builder with minimal services injected (PhotinoWindow, IOptions<PhotinoDevelopmentServerOptions>)
        var builder = PhotinoApplicationBuilder.CreatePhotinoBuilder(args, out var url);

        // Adds the IPCService inside the depdendency container
        builder.Services.AddInterProcessCommunication();

        // Configure the IOptions<PhotinoDevelopmentServerOptions>
        builder.Services.Configure<PhotinoDevelopmentServerOptions>(options =>
        {
            // We bump up the "WaitUntilReadyTimeout" to 30 seconds, default 10 seconds
            options.WaitUntilReadyTimeout = TimeSpan.FromSeconds(30);
        });

        var photino = builder.BuildApplication();

        photino.Window
            .Center()
            .SetTitle("Photino.Final.Sample")
            .SetSize(800, 600)
            .SetUseOsDefaultSize(false)
            // Registers a channel with a payload of type string named "PHOTINO_TEST_CHANNEL"
            .RegisterChannel<string>("PHOTINO_TEST_CHANNEL", (sender, message) =>
            {
                Console.WriteLine($"Received {message} from IPC Renderer");
                sender.Emit("Hello from .NET 🤖");
            });

        /* 
            Loads the framework development server (React, Vite, Angular, etc.) if DOTNET_ENVIRONMENT = "Development"
            inside the environment variables of the launchSettings.json. Else it loads the compiled version in the wwwroot
            folder of the Publish directory, this is written in the .csproj file
        */
        photino.Run(url);
    }
}