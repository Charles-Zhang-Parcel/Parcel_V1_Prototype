using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PreviewWindow : BaseWindow
    {
        #region Construction
        public PreviewWindow(Window owner, ProcessorNode processorNode)
        {
            Owner = owner;
            Node = processorNode;
            InitializeComponent();
        }
        public ProcessorNode Node { get; }
        #endregion

        #region View Properties
        private string _testLabel;
        public string TestLabel
        {
            get => _testLabel;
            set => SetField(ref _testLabel, value);
        }
        #endregion

        #region Interface
        public void Update()
        {
            UpdateLayout();
        }
        #endregion
    }
}