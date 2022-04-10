using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Parcel.FrontEnd.NodifyWPF.SpecialNodes;
using Parcel.Shared;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Toolbox.Basic;
using Parcel.Toolbox.ControlFlow;
using Parcel.Toolbox.DataProcessing;
using Parcel.Toolbox.FileSystem;
using Parcel.Toolbox.Finance;
using ToolboxDefinition = Parcel.Toolbox.Basic.ToolboxDefinition;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PopupTab : BaseWindow
    {
        ToolboxRegistry registry = new ToolboxRegistry();
        public PopupTab(Window owner)
        {
            registry.RegisterToolbox("Basic", Assembly.GetAssembly(typeof(Parcel.Toolbox.Basic.ToolboxDefinition)));
            registry.RegisterToolbox("Control Flow", Assembly.GetAssembly(typeof(Parcel.Toolbox.ControlFlow.ToolboxDefinition)));
            registry.RegisterToolbox("Data Processing", Assembly.GetAssembly(typeof(Parcel.Toolbox.DataProcessing.ToolboxDefinition)));
            registry.RegisterToolbox("File System", Assembly.GetAssembly(typeof(Parcel.Toolbox.FileSystem.ToolboxDefinition)));
            registry.RegisterToolbox("Finance", Assembly.GetAssembly(typeof(Parcel.Toolbox.Finance.ToolboxDefinition)));
            registry.RegisterToolbox("Special", Assembly.GetAssembly(typeof(Parcel.FrontEnd.NodifyWPF.SpecialNodes.GraphToolboxDefinition)));
            registry.RegisterToolbox("Data Source", Assembly.GetAssembly(typeof(Parcel.Toolbox.DataSource.ToolboxDefinition)));
            registry.RegisterToolbox("Plotting", Assembly.GetAssembly(typeof(Parcel.Toolbox.Plotting.ToolboxDefinition)));
            registry.RegisterToolbox("Graphing", Assembly.GetAssembly(typeof(Parcel.Toolbox.Graphing.ToolboxDefinition)));
            registry.RegisterToolbox("Present", Assembly.GetAssembly(typeof(Parcel.Toolbox.Present.ToolboxDefinition)));

            Owner = owner;
            InitializeComponent();
            
            foreach (string toolbox in registry.Toolboxes.Keys.OrderBy(k => k))
            {
                var menu = new Menu()
                {
                    // Margin = new Thickness(1)
                };
                var topMenu = new MenuItem()
                {
                    // Padding = new Thickness(4),
                    Header = toolbox,
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
                    if (node == null)
                    {
                        topMenu.Items.Add(new Separator());
                    }
                    else
                    {
                        var item = new MenuItem()
                        {
                            Header = node.Name
                        };
                        item.Tag = node;
                    
                        item.Click += NodeMenuItemOnClick;
                        topMenu.Items.Add(item);   
                    }
                }
                modulesList.Items.Add(menu);
            }
        }
        
        #region Interface
        public ToolboxNodeExport ToolSelection { get; set; }
        public Action<ToolboxNodeExport> ItemSelected { get; set; }
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
            ItemSelected(ToolSelection);
            Close();
        }
        #endregion
    }
}