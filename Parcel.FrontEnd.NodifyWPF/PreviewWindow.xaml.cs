using System.Windows;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PreviewWindow : BaseWindow
    {
        public PreviewWindow(Window owner)
        {
            Owner = owner;
            InitializeComponent();
        }
    }
}