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

        private void PopupTab_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void PopupTab_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                this.Close();
        }
    }
}