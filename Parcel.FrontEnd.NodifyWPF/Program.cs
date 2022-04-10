using System;
using System.Windows.Threading;

namespace Parcel.FrontEnd.NodifyWPF
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Parcel.FrontEnd.NodifyWPF.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}