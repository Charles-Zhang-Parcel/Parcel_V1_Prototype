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
            RepeatLastCommand = new DelegateCommand(() => SpawnNode(LastTool, Editor.MouseLocation), () => LastTool != null);
            SaveCanvasCommand = new DelegateCommand(() => SaveCanvas(), () => Canvas.Nodes.Count != 0);
            OpenCanvasCommand = new DelegateCommand(() => OpenCanvas(), () => true);
            OpenWebHostCommand = new DelegateCommand(() => WebHostRuntime.Singleton.Open(), () => true);

            // WebAccessPointUrl = WebHostRuntime.Singleton?.BaseUrl;
            
            InitializeComponent();
            
            EventManager.RegisterClassHandler(typeof(Nodify.BaseConnection), MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnConnectionInteraction));
            EventManager.RegisterClassHandler(typeof(Nodify.Node), MouseLeftButtonDownEvent, new MouseButtonEventHandler(NodeDoubleclick_OpenProperties));
            EventManager.RegisterClassHandler(typeof(Nodify.GroupingNode), MouseLeftButtonDownEvent, new MouseButtonEventHandler(NodeDoubleclick_OpenProperties));
        }
        public NodesCanvas Canvas { get; set; } = new NodesCanvas();
        #endregion

        #region Commands
        private ToolboxNodeExport LastTool { get; set; }
        public ICommand RepeatLastCommand { get; }
        public ICommand SaveCanvasCommand { get; }
        public ICommand OpenCanvasCommand { get; }
        public ICommand OpenWebHostCommand { get; }
        #endregion

        #region View Properties
        private string _webAccessPointUrl;
        public string WebAccessPointUrl { get => _webAccessPointUrl; set => SetField(ref _webAccessPointUrl, value); }
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
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                ShowSearchNodePopup();
                e.Handled = true;
            }
        }
        private void Editor_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowSearchNodePopup();
            e.Handled = true;
        }
        private void NodeDoubleclick_OpenProperties(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            if (!(e.Source is Nodify.Node {DataContext: ProcessorNode processorNode})
                && !(e.Source is Nodify.GroupingNode {DataContext: CommentNode commentNode})) return;

            if (e.Source is Nodify.Node node)
                SpawnPropertyWindow(node.DataContext as BaseNode);
            else if (e.Source is Nodify.GroupingNode groupingNode)
                SpawnPropertyWindow(groupingNode.DataContext as CommentNode);
            
            e.Handled = true;
        }
        private void OpenFileNode_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is Button {DataContext: OpenFileNode node})) return;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                node.Path = openFileDialog.FileName;
            }
            e.Handled = true;
        }
        private void ProcessorNodeTogglePreviewButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Border {Tag: ProcessorNode node} border)) return;

            node.IsPreview = !node.IsPreview;
            e.Handled = true;
        }
        private void ProcessorNodePreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button {Tag: ProcessorNode node} button)) return;

            // Auto-Generate
            if ((node is CSV || node is Excel) && node.ShouldHaveConnection)
            {
                OpenFileNode filePathNode = SpawnNode(new ToolboxNodeExport("File Input", typeof(OpenFileNode)),
                    node.Location + new Vector(-200, -60)) as OpenFileNode;
                Canvas.Schema.TryAddConnection(filePathNode!.FilePathOutput, node.Input.First());
                
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (node is CSV)
                    openFileDialog.Filter = "CSV file (*.csv)|*.csv|All types (*.*)|*.*";
                else if (node is Excel)
                    openFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx|All types (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    filePathNode.Path = openFileDialog.FileName;
                }
            }
            else if (node is IAutoConnect autoConnect && autoConnect.ShouldHaveConnection && node.AutoGenerateNodes != null)
            {
                foreach (Tuple<ToolboxNodeExport,Vector,InputConnector> generateNode in autoConnect.AutoGenerateNodes)
                {
                    BaseNode temp = SpawnNode(generateNode.Item1, node.Location + generateNode.Item2);
                    if(temp is IMainOutputNode outputNode)
                        Canvas.Schema.TryAddConnection(outputNode.MainOutput, generateNode.Item3);
                }
                
                e.Handled = true;
                return;
            }
            
            // Connection check
            if (node.ShouldHaveConnection && node.AutoGenerateNodes == null)
            {
                node.Message.Content = "Require Connection.";
                node.Message.Type = NodeMessageType.Error;
                
                e.Handled = true;
                return;
            }
            
            // TODO: This is a good chance to auto-save, before anything can crash
            
            node.IsPreview = true;
            SpawnPreviewWindow(node);
            ExecuteAll();

            e.Handled = true;
        }
        private void WebAccessUrlDisplayElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WebHostRuntime.Singleton.Open();
            e.Handled = true;
        }
        private void HostAddressNodeUIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            WebHostRuntime.Singleton.Open((sender as Label).Content as string);
            e.Handled = true;
        }
        #endregion

        #region Routine
        private BaseNode SpawnNode(ToolboxNodeExport tool, Point spawnLocation)
        {
            BaseNode node = (BaseNode) Activator.CreateInstance(tool.Type);
            node!.Location = spawnLocation;
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
        private void SpawnPropertyWindow(BaseNode node)
        {
            Point cursor = GetCurosrWindowPosition();
            if(node is ProcessorNode processorNode)
                new PropertyWindow(this, processorNode)
                {
                    Left = cursor.X,
                    Top = cursor.Y
                }.Show();
            else if(node is CommentNode commentNode)
                new CommentWindow(this, commentNode)
                {
                    Left = cursor.X,
                    Top = cursor.Y
                }.Show();
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
            Point cursor = GetCurosrWindowPosition();
            Point spawnLocation = Editor.MouseLocation;

            PopupTab popupTab = new PopupTab(this)
            {
                Left = cursor.X,
                Top = cursor.Y,
                Topmost = true
            };
            void action(ToolboxNodeExport toolboxNodeExport)
            {
                if (toolboxNodeExport != null 
                    && toolboxNodeExport.Type != typeof(object)) // Don't do anything for placeholder nodes
                {
                    LastTool = toolboxNodeExport;
                    SpawnNode(LastTool, spawnLocation);
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

        private Point GetCurosrWindowPosition()
        {
            Point cursor = Mouse.GetPosition(this);
            RECT rect = GetWindowRectangle(this);
            return new Point((this.WindowState == WindowState.Maximized ? rect.Left : this.Left) + cursor.X,
                (this.WindowState == WindowState.Maximized ? rect.Top : this.Top) + cursor.Y);
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