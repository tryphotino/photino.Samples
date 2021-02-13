using PhotinoNET;
using System;

namespace HelloPhotino.Angular
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var window = new PhotinoWindow("Hello Photino Angular!"
                , options => { }
                , x:10
                , y:10
                , width:900
                , height:800);

            window.NavigateToLocalFile("wwwroot/index.html");
         
            window.WaitForExit();
        }
    }
}
