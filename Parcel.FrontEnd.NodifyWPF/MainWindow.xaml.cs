using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nodify;
using Parcel.Shared;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Toolbox.Basic;
using Parcel.Toolbox.ControlFlow;
using Parcel.Toolbox.DataProcessing;
using Parcel.Toolbox.FileSystem;
using Parcel.Toolbox.Finance;
using BaseConnection = Parcel.Shared.Framework.ViewModels.BaseConnection;
using PendingConnection = Parcel.Shared.Framework.ViewModels.PendingConnection;

namespace Parcel.FrontEnd.NodifyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : BaseWindow
    {
        #region Constructor
        public MainWindow()
        {
            RepeatLastCommand = new DelegateCommand(() => SpawnNode(LastTool), () => LastTool != null);
            
            InitializeComponent();
        }
        public NodesCanvas Canvas { get; set; } = new NodesCanvas();
        #endregion

        #region Commands
        private ToolboxNodeExport LastTool { get; set; }
        public ICommand RepeatLastCommand { get; }
        #endregion

        #region Events
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                var cursor = Mouse.GetPosition(this);

                PopupTab popupTab = new PopupTab()
                {
                    Left = this.Left + cursor.X,
                    Top = this.Top + cursor.Y
                };
                popupTab.ShowDialog();
                if (popupTab.ToolSelection != null)
                {
                    LastTool = popupTab.ToolSelection;
                    SpawnNode(LastTool);
                }
            }
        }
        #endregion

        #region Routine

        private void SpawnNode(ToolboxNodeExport tool)
        {
            BaseNode node = (BaseNode) Activator.CreateInstance(LastTool.Type);
            Canvas.Nodes.Add(node);
        }
        #endregion
    }
}