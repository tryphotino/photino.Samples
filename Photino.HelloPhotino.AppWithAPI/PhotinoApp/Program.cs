using System;
using System.IO;
using System.Text;
using PhotinoAPI;
using PhotinoNET;

namespace PhotinoApp
{
    class Program
    {
        // API Paths
        private static string _apiBaseUri = "http://localhost:3300";
        private static string _apiEnvironment = "Development";

        // UI Resource Paths
        private static string _wwwrootPath = "./wwwroot";
        private static string _assetsPath = $"{_wwwrootPath}/assets";
        private static string _cssAssetsPath = $"{_assetsPath}/css";
        private static string _jsAssetsPath = $"{_assetsPath}/js";

        // Custom schemes
        private static string _cssAssetScheme = "css-asset";
        private static string _jsAssetScheme = "js-asset";

        [STAThread]
        static void Main(string[] args)
        {
            // API host configuration
            string[] apiArgs = new string[] {
                $"URLS={_apiBaseUri}",
                $"ENVIRONMENT={_apiEnvironment}"
            };

            // Prepare the API host of the API project
            using (var api = PhotinoAPI.Program.CreateHostBuilder(apiArgs).Build())
            {
                // Start API host
                api.StartAsync();

                // Window configuration
                string windowTitle = "Photino.NET Demo App + ASP.NET Core API";

                // Prepare the PhotinoWindow instance
                var window = new PhotinoWindow(windowTitle, WindowConfigurationAction)
                    .Resize(1280, 800)
                    .Center()
                    .UserCanResize(false)
                    .Load($"{_wwwrootPath}/index.html");

                window.WaitForClose(); // Starts the application event loop
            }
        }

        private static Action<PhotinoWindowOptions> WindowConfigurationAction = options =>
        {
            options.CustomSchemeHandlers.Add(_cssAssetScheme, CreateAssetSchemeDelegate(_cssAssetScheme, _cssAssetsPath, "text/css"));
            
            options.CustomSchemeHandlers.Add(
                _jsAssetScheme,
                CreateAssetSchemeDelegate(_jsAssetScheme, _jsAssetsPath, "application/javascript", (string filepath) =>
                {
                    // Inject dynamic JavaScript for specific filepath
                    if (filepath.Contains("/photino.js"))
                    {
                        return $@"
                            const photinoApiBaseUri = '{_apiBaseUri}';
                            console.log(`Photino Base API Uri: ${{photinoApiBaseUri}}`);
                        ";
                    }

                    return ""; // default
                })
            );
        };

        private static CustomSchemeDelegate CreateAssetSchemeDelegate(string scheme, string assetsPath, string returnContentType, Func<string, string> overrideDefaultContentDelegate = null)
        {
            return (string url, out string contentType) =>
            {
                contentType = returnContentType;

                string content = "";
                string filename = url.Replace($"{scheme}://", "");
                string filepath = $"{assetsPath}/{filename}";

                if (File.Exists(filepath))
                {
                    content = File.ReadAllText(filepath);
                }
                else if (overrideDefaultContentDelegate != null)
                {
                    content = overrideDefaultContentDelegate(filepath);
                }

                return new MemoryStream(Encoding.UTF8.GetBytes(content));
            };
        }
    }
}
