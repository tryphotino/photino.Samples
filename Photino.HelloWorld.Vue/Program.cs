using System;

namespace HelloWorldVue
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoNET.PhotinoNET("HelloWorldVue App", options =>
            {
                // options.SchemeHandlers.Add("file", CreateFileSchemeHandler);
            });

            // window.OnWebMessageReceived += (sender, message) =>
            // {
            //     window.SendMessage("Got message: " + message);
            // };

            window.NavigateToLocalFile("wwwroot/index.html");
            window.WaitForExit();
        }

        // static MemoryStream CreateFileSchemeHandler(string url, out string contentType)
        // {
        //     string path = url.Replace("file://", "");
        //     contentType = GetContentType(path);
        //     return GetFile($"./wwwroot/{path}");
        // }

        // static MemoryStream GetFile(string path)
        // {
        //     if (File.Exists(path) == false)
        //     {
        //         throw new FileNotFoundException($"File {path} does not exist.");
        //     }

        //     var contents = File.ReadAllText(path);
        //     return GetMemoryStream(contents);
        // }

        // static MemoryStream GetMemoryStream(string s)
        // {
        //     return new MemoryStream(Encoding.UTF8.GetBytes(s));
        // }

        // static string GetContentType(string path)
        // {
        //     string contentType;
        //     new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
        //     return contentType ?? "application/octet-stream";
        // }
    }
}
