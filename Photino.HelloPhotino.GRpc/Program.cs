using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PhotinoNET;
using System;
using System.IO;
using System.Text;

namespace HelloPhotino.GRpc
{
    //https://github.com/grpc/grpc-web/tree/master/net/grpc/gateway/examples/helloworld#write-client-code
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().RunAsync(); ;

            var window = new PhotinoWindow("Hello Photino gRPC Enabled!"
                , options =>
                    {
                        options.CustomSchemeHandlers.Add("app", (string url, out string contentType) =>
                        {
                            //This code demonstrates setting up a 'channel' to inject JavaScript.
                            //See the <script src="app://something.js"></script> tag in Index.html
                            contentType = "text/javascript";
                            return new MemoryStream(Encoding.UTF8.GetBytes("alert('super')"));
                        });
                    }
                , left:5
                , top:5);

            window.RegisterWebMessageReceivedHandler((sender, message) =>
            {
                window.SendWebMessage("Got message: " + message);
            });

            window.Load("wwwroot/index.html");
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
