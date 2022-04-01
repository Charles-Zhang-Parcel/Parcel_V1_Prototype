using System.Windows;
using System.Windows.Input;

namespace Parcel.FrontEnd.NodifyWPF
{
    public partial class PopupTab : Window
    {
        public PopupTab()
        {
            InitializeComponent();
        }

        #region Interface
        public string ToolSelection { get; set; }
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
        #endregion

        #region Test Events
        private void MenuItem_Label_Click(object sender, RoutedEventArgs e)
        {
            ToolSelection = "Label";
            Close();
        }

        private void MenuItem_CSV_Click(object sender, RoutedEventArgs e)
        {
            ToolSelection = "CSV";
            Close();
        }
        #endregion
    }
}