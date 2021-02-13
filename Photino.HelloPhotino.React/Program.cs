using PhotinoNET;
using System;

namespace HelloPhotino.React
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino React App"
                , options => { }
                , x:15
                , y:15
                , width:600
                , height:400);

            window.NavigateToLocalFile("wwwroot/index.html");

            window.WaitForExit();
        }
    }
}
