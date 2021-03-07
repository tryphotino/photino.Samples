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
                , left:15
                , top:15
                , width:600
                , height:400);

            window.Load("wwwroot/index.html");

            window.WaitForClose();
        }
    }
}
