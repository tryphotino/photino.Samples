using Photino.NET;

namespace PhotinoApp;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        PhotinoWindow main = new PhotinoWindow()
            .SetTitle("Photino App")
            .SetUseOsDefaultSize(false)
            .SetSize(768, 512)
            .Load("wwwroot/index.html");

        main.WaitForClose();
    }
}