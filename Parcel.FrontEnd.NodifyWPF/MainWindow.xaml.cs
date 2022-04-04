﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using Microsoft.Win32;
using Nodify;
using Parcel.Shared;
using Parcel.Shared.Algorithms;
using Parcel.Shared.Framework;
using Parcel.Shared.Framework.ViewModels;
using Parcel.Shared.Framework.ViewModels.BaseNodes;
using Parcel.Shared.Framework.ViewModels.Primitives;
using Parcel.Toolbox.Basic;
using Parcel.Toolbox.ControlFlow;
using Parcel.Toolbox.DataProcessing;
using Parcel.Toolbox.DataProcessing.Nodes;
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

                PopupTab popupTab = new PopupTab(this)
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
        private void NodeDoubleclick_OpenProperties(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is Nodify.Node {DataContext: ProcessorNode node})) return;
            
            new PropertyWindow(this, node).Show();
        }
        private void OpenFileNode_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button {DataContext: OpenFileNode node})) return;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                node.Path = openFileDialog.FileName;
            }
        }
        private void ProcessorNodeTogglePreviewButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Border {Tag: ProcessorNode node} border)) return;

            node.IsPreview = !node.IsPreview;
        }
        private void ProcessorNodePreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button {Tag: ProcessorNode node} button)) return;

            node.IsPreview = true;
            
            // Auto-Generate
            if ((node is CSV || node is DataTable)
                && node.Input.All(i => i.Connections.Count == 0))
            {
                OpenFileNode filePathNode = SpawnNode(new ToolboxNodeExport("File Input", typeof(OpenFileNode))) as OpenFileNode;
                Canvas.Schema.TryAddConnection(filePathNode!.FilePathOutput,
                    (node is CSV csv) ? csv.PathInput : (node as DataTable).PathInput);
                
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    filePathNode.Path = openFileDialog.FileName;
                }
            }
            
            SpawnPreviewWindow(node);
            ExecuteAll();
        }
        #endregion

        #region Routine
        private BaseNode SpawnNode(ToolboxNodeExport tool)
        {
            BaseNode node = (BaseNode) Activator.CreateInstance(tool.Type);
            Canvas.Nodes.Add(node);
            return node;
        }
        private void SpawnPreviewWindow(ProcessorNode node)
        {
            PreviewWindow preview = new PreviewWindow(this, node);
            _previewWindows.Add(preview);
            preview.Closed += (sender, args) => _previewWindows.Remove(sender as PreviewWindow); 
            preview.Show();
        }
        private void ExecuteAll()
        {
            var processors = Canvas.Nodes
                .Where(n => n is ProcessorNode node && node.IsPreview == true)
                .Select(n => n as ProcessorNode);
            
            IExecutionGraph graph = new ExecutionQueue();
            graph.InitializeGraph(processors);
            graph.ExecuteGraph();
            _previewWindows.ForEach(p => p.Update());
        }
        #endregion

        #region State
        private readonly List<PreviewWindow> _previewWindows = new List<PreviewWindow>();
        #endregion
    }
}