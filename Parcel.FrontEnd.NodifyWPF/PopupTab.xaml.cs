using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Parcel.Shared;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

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
            registry.RegisterToolbox("Math", Assembly.GetAssembly(typeof(Parcel.Toolbox.Math.ToolboxDefinition)));
            registry.RegisterToolbox("Logic", Assembly.GetAssembly(typeof(Parcel.Toolbox.Logic.ToolboxDefinition)));
            registry.RegisterToolbox("Plotting", Assembly.GetAssembly(typeof(Parcel.Toolbox.Plotting.ToolboxDefinition)));
            registry.RegisterToolbox("Graphing", Assembly.GetAssembly(typeof(Parcel.Toolbox.Graphing.ToolboxDefinition)));
            registry.RegisterToolbox("Present", Assembly.GetAssembly(typeof(Parcel.Toolbox.Present.ToolboxDefinition)));

            Owner = owner;
            InitializeComponent();
            
            foreach (string name in registry.Toolboxes.Keys.OrderBy(k => k))
            {
                Menu menu = new Menu()
                {
                    // Margin = new Thickness(1)
                };
                MenuItem topMenu = new MenuItem
                {
                    // Padding = new Thickness(4),
                    Header = name, 
                    Width = this.Width * 0.8,
                };
                menu.Items.Add(topMenu);
                
                string formalName = $"{name.Replace(" ", String.Empty)}"; 
                string toolboxHelperTypeName = $"Parcel.Toolbox.{formalName}.{formalName}Helper";
                IToolboxEntry toolbox = (IToolboxEntry)Activator.CreateInstance(registry.Toolboxes[name]
                    .GetTypes().Single(p => typeof(IToolboxEntry).IsAssignableFrom(p)));
                if (toolbox != null)
                {
                    foreach (ToolboxNodeExport node in toolbox.ExportNodes)
                        AddMenuItem(node, topMenu);
                    foreach (AutomaticNodeDescriptor definition in toolbox.AutomaticNodes)
                        AddMenuItem(definition == null ? null : new ToolboxNodeExport(definition.NodeName, typeof(AutomaticProcessorNode))
                        {
                            Descriptor = definition,
                            Toolbox = toolbox,
                        }, topMenu);
                }

                modulesList.Items.Add(menu);
            }
        }

        #region Routines
        private void AddMenuItem(ToolboxNodeExport node, MenuItem topMenu)
        {
            if (node == null)
                topMenu.Items.Add(new Separator());
            else
            {
                MenuItem item = new MenuItem {Header = node.Name, Tag = node};
                item.Click += NodeMenuItemOnClick;
                topMenu.Items.Add(item);
            }
        }
        #endregion

        #region Interface
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
            
            ToolboxNodeExport toolSelection = item.Tag as ToolboxNodeExport;
            ItemSelected(toolSelection);
            Close();
        }
        #endregion
    }
}