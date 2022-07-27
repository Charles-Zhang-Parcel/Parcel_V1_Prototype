using Parcel.Shared.Framework.ViewModels;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Parcel.FrontEnd.NodifyWPF
{
    internal class CommandManagerWrapper : ICommandManager
    {
        public void AddEvent(EventHandler e)
        {
            CommandManager.RequerySuggested += e;
        }

        public void RemoveEvent(EventHandler e)
        {
            CommandManager.RequerySuggested -= e;
        }
    }

    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // charles-z 20220726: Quick hack to get around dependancy
            CommandManagerWrapper wrapper = new CommandManagerWrapper();
            RequeryCommand.CommandManager = wrapper;
            RequeryCommand.CommandManager = wrapper;

            var app = new Parcel.FrontEnd.NodifyWPF.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}