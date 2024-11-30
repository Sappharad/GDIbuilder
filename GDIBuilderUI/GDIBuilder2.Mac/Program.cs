using System;
using Eto.Forms;

namespace GDIBuilder2.Mac
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Eto.Style.Add<Eto.Mac.Forms.ApplicationHandler>(null, handler => handler.AllowClosingMainForm = true);
            new Application(Eto.Platforms.Mac64).Run(new MainForm());
        }
    }
}