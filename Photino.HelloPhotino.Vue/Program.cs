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
                , left:20
                , top:20
                , width:800
                , height:700);

            window.Load("wwwroot/index.html");

            window.WaitForClose();
        }
    }
}
