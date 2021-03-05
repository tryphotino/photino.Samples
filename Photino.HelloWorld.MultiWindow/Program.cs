using System;
using PhotinoNET;

namespace HelloWorldApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var mainWindow = new PhotinoWindow("Main Window")
                .RegisterWebMessageReceivedHandler(CloseWindowMessageDelegate)
                .RegisterWebMessageReceivedHandler(NewWindowMessageDelegate)
                .Resize(600, 400)
                .Center();

            mainWindow
                .Load("wwwroot/main.html")
                .WaitForClose();
        }

        static void CloseWindowMessageDelegate(object sender, string message)
        {
            var window = (PhotinoWindow)sender;
            
            if (message == "close-window")
            {
                Console.WriteLine($"Closing \"{window.Title}\".");
                window.Close();
            }
        }

        static void NewWindowMessageDelegate(object sender, string message)
        {
            var window = (PhotinoWindow)sender;

            if (message == "random-window")
            {
                var random = new Random();

                int workAreaWidth = window.MainMonitor.WorkArea.Width;
                int workAreaHeight = window.MainMonitor.WorkArea.Height;

                int width = random.Next(400, 800);
                int height = (int)Math.Round(width * 0.625, 0);

                int offset = 20;
                int left = random.Next(offset, workAreaWidth - width - offset);
                int top = random.Next(offset, workAreaHeight - height - offset);

                Action<PhotinoWindowOptions> randomWindowConfiguration = options =>
                {
                    options.Parent = window;
                };

                new PhotinoWindow($"Random Window ({window.Children.Count + 1})", randomWindowConfiguration, width, height, left, top)
                    .RegisterWebMessageReceivedHandler(CloseWindowMessageDelegate)
                    .Load("wwwroot/random.html")
                    .WaitForClose();
            }
        }
    }
}
