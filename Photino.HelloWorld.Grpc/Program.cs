using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PhotinoNET;
using System;
using System.IO;
using System.Text;

namespace HelloPhotinoGRpc
{
    //https://github.com/grpc/grpc-web/tree/master/net/grpc/gateway/examples/helloworld#write-client-code
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().RunAsync(); ;

            var window = new PhotinoWindow("Hello Photino gRPC Enabled App", options =>
            {
                options.SchemeHandlers.Add("app", (string url, out string contentType) =>
                {
                    //This code demonstrates setting up a 'channel' to inject JavaScript.
                    //See the <script src="app://something.js"></script> tag in Index.html
                    contentType = "text/javascript";
                    return new MemoryStream(Encoding.UTF8.GetBytes("alert('super')"));
                });
            });

            window.OnWebMessageReceived += (sender, message) =>
            {
                window.SendMessage("Got message: " + message);
            };

            window.NavigateToLocalFile("wwwroot/index.html");
            window.WaitForExit();
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
