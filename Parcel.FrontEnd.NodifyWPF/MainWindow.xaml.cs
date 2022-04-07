using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
            SaveCanvasCommand = new DelegateCommand(() => SaveCanvas(), () => Canvas.Nodes.Count != 0);
            OpenCanvasCommand = new DelegateCommand(() => OpenCanvas(), () => true);
            
            InitializeComponent();
            
            EventManager.RegisterClassHandler(typeof(Nodify.BaseConnection), MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnConnectionInteraction));
            EventManager.RegisterClassHandler(typeof(Nodify.Node), MouseDoubleClickEvent, new MouseButtonEventHandler(NodeDoubleclick_OpenProperties));
        }
        public NodesCanvas Canvas { get; set; } = new NodesCanvas();
        #endregion

        #region Commands
        private ToolboxNodeExport LastTool { get; set; }
        public ICommand RepeatLastCommand { get; }
        public ICommand SaveCanvasCommand { get; }
        public ICommand OpenCanvasCommand { get; }
        #endregion

        #region Advanced Node Graph Behaviors
        private void OnConnectionInteraction(object sender, MouseButtonEventArgs e)
        {
            if (sender is Nodify.BaseConnection ctrl && ctrl.DataContext is BaseConnection connection)
            {
                if (Keyboard.Modifiers == ModifierKeys.Alt)
                {
                    connection.Remove();
                }
                else if (e.ClickCount > 1)
                {
                    connection.Split(e.GetPosition(ctrl) - new Vector(30, 15));
                }
            }
        }
        #endregion

        #region Events
        private void PrimitiveInputConnectorEntryModificationButton_DisableOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
               ShowSearchNodePopup();
        }
        private void Editor_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
            => ShowSearchNodePopup();
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
            if ((node is CSV csvNode && csvNode.PathInput.Connections.Count == 0)
                || (node is DataTable dataTableNode && dataTableNode.PathInput.Connections.Count == 0))
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
            
            // TODO: This is a good chance to auto-save, before anything can crash
            
            SpawnPreviewWindow(node);
            ExecuteAll();
        }
        #endregion

        #region Routine
        private BaseNode SpawnNode(ToolboxNodeExport tool)
        {
            BaseNode node = (BaseNode) Activator.CreateInstance(tool.Type);
            node!.Location = Editor.MouseLocation;
            Canvas.Nodes.Add(node);
            return node;
        }
        private void SpawnPreviewWindow(ProcessorNode node)
        {
            if (_previewWindows.ContainsKey(node))
            {
                _previewWindows[node].Activate();
            }
            else
            {
                PreviewWindow preview = new PreviewWindow(this, node);
                _previewWindows.Add(node, preview);
                preview.Closed += (sender, args) => _previewWindows.Remove((sender as PreviewWindow)!.Node); 
                preview.Show();   
            }
        }
        private void ExecuteAll()
        {
            var processors = Canvas.Nodes
                .Where(n => n is ProcessorNode node && node.IsPreview == true)
                .Select(n => n as ProcessorNode);
            
            IExecutionGraph graph = new ExecutionQueue();
            graph.InitializeGraph(processors);
            graph.ExecuteGraph();
            foreach (PreviewWindow p in _previewWindows.Values)
                p.Update();
        }
        private void ShowSearchNodePopup()
        {
            var cursor = Mouse.GetPosition(this);
            var rect = GetWindowRectangle(this);

            PopupTab popupTab = new PopupTab(this)
            {
                Left = this.WindowState == WindowState.Maximized ? rect.Left : this.Left + cursor.X,
                Top = this.WindowState == WindowState.Maximized ? rect.Top : this.Top + cursor.Y,
                Topmost = true
            };
            void action(ToolboxNodeExport toolboxNodeExport)
            {
                if (toolboxNodeExport != null)
                {
                    LastTool = toolboxNodeExport;
                    SpawnNode(LastTool);
                }
            }
            popupTab.ItemSelected += action;
            popupTab.MouseLeave += delegate { popupTab.Close(); };  // ISSUE: This will cause Closed event being called before the OnClick event
            popupTab.Show();
        }
        private void OpenCanvas()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Parcel workflow file (*.parcel)|*.parcel|YAML file (*.yaml)|*.yaml";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                Canvas.Open(path);
            }
        }
        private void SaveCanvas()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Parcel workflow file (*.parcel)|*.parcel|YAML file (*.yaml)|*.yaml";
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                Canvas.Save(path);
            }
        }
        #endregion

        #region State
        private readonly Dictionary<ProcessorNode, PreviewWindow> _previewWindows =
            new Dictionary<ProcessorNode, PreviewWindow>();
        #endregion

        #region Interop
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // Make sure RECT is actually OUR defined struct, not the windows rect.
        public static RECT GetWindowRectangle(Window window)
        {
            RECT rect;
            GetWindowRect((new WindowInteropHelper(window)).Handle, out rect);

            return rect;
        }
        #endregion
    }
}