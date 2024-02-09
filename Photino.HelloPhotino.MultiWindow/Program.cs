using PhotinoNET;
using System;

namespace HelloWorldApp
{
    class Program
    {
        static int _childCount;

        [STAThread]
        static void Main(string[] args)
        {
            new PhotinoWindow()
                .SetTitle("Main Window")
                .RegisterWebMessageReceivedHandler(CloseWindowMessageDelegate)
                .RegisterWebMessageReceivedHandler(NewWindowMessageDelegate)
                .SetUseOsDefaultSize(false)
                .SetWidth(600)
                .SetHeight(400)
                .Center()
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
            var parent = (PhotinoWindow)sender;

            if (message == "random-window")
            {
                var random = new Random();

                int workAreaWidth = parent.MainMonitor.WorkArea.Width;
                int workAreaHeight = parent.MainMonitor.WorkArea.Height;

                int width = random.Next(400, 800);
                int height = (int)Math.Round(width * 0.625, 0);

                int offset = 20;
                int left = random.Next(offset, workAreaWidth - width - offset);
                int top = random.Next(offset, workAreaHeight - height - offset);

                _childCount++;

                new PhotinoWindow(parent)
                    .SetTitle($"Random Window ({_childCount})")
                    .SetUseOsDefaultSize(false)
                    .SetHeight(height)
                    .SetWidth(width)
                    .SetUseOsDefaultLocation(false)
                    .SetTop(top)
                    .SetLeft(left)
                    .RegisterWebMessageReceivedHandler(CloseWindowMessageDelegate)
                    .Load("wwwroot/random.html")
                    .WaitForClose();
            }
        }
    }
}
