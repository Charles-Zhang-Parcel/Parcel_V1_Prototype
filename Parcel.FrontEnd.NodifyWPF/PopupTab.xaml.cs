using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Parcel.Shared;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Toolbox.Basic;
using Parcel.Toolbox.ControlFlow;
using Parcel.Toolbox.DataProcessing;
using Parcel.Toolbox.FileSystem;
using Parcel.Toolbox.Finance;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PopupTab : BaseWindow
    {
        ToolboxRegistry registry = new ToolboxRegistry();
        public PopupTab()
        {
            registry.RegisterToolbox("Basic", Assembly.GetAssembly(typeof(BasicHelper)));
            registry.RegisterToolbox("Control Flow", Assembly.GetAssembly(typeof(ControlFlowHelper)));
            registry.RegisterToolbox("Data Processing", Assembly.GetAssembly(typeof(DataProcessingHelper)));
            registry.RegisterToolbox("File System", Assembly.GetAssembly(typeof(FileSystemHelper)));
            registry.RegisterToolbox("Finance", Assembly.GetAssembly(typeof(FinanceHelper)));

            InitializeComponent();
            
            foreach (string toolbox in registry.Toolboxes.Keys.OrderBy(k => k))
            {
                var menu = new Menu();
                var topMenu = new MenuItem()
                {
                    Header = toolbox
                };
                topMenu.Width = this.Width * 0.8;
                menu.Items.Add(topMenu);
                
                var formalName = $"{toolbox.Replace(" ", String.Empty)}"; 
                var toolboxHelperTypeName = $"Parcel.Toolbox.{formalName}.{formalName}Helper";
                var type = typeof(IToolboxEntry);
                var definition = (IToolboxEntry)Activator.CreateInstance(registry.Toolboxes[toolbox]
                    .GetTypes().Single(p => type.IsAssignableFrom(p)));
                foreach (ToolboxNodeExport node in definition.ExportNodes)
                {
                    var item = new MenuItem()
                    {
                        Header = node.Name
                    };
                    item.Tag = node;
                    
                    item.Click += NodeMenuItemOnClick;
                    topMenu.Items.Add(item);
                }
                modulesList.Items.Add(menu);
            }
        }
        
        #region Interface
        public ToolboxNodeExport ToolSelection { get; set; }
        #endregion

        #region View Properties
        #endregion

        #region GUI Events
        private void PopupTab_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void PopupTab_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.Close();
        }
        private void NodeMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is MenuItem item) || item.Tag == null) return;
            
            ToolSelection = item.Tag as ToolboxNodeExport;
            Close();
        }
        #endregion
    }
}