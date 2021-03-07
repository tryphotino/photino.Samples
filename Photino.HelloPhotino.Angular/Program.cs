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
                , left:10
                , top:10
                , width:900
                , height:800);

            window.Load("wwwroot/index.html");
         
            window.WaitForClose();
        }
    }
}
