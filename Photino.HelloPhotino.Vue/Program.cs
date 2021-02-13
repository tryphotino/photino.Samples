using PhotinoNET;
using System;

namespace HelloPhotino.Vue
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino Vue!"
                , options => { }
                , x:20
                , y:20
                , width:800
                , height:700);

            window.NavigateToLocalFile("wwwroot/index.html");

            window.WaitForExit();
        }
    }
}
