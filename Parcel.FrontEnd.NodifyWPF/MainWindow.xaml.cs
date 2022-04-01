using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using Nodify;
using Parcel.FrontEnd.NodifyWPF.ViewModels;
using Parcel.FrontEnd.NodifyWPF.ViewModels.BaseNodes;

namespace Parcel.FrontEnd.NodifyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Public View Properties
        public IEnumerable<BaseNode> Nodes { get; } = new List<BaseNode>
        {
        };
        #endregion
        
        #region Events
        private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                var cursor = Mouse.GetPosition(this);

                PopupTab popupTab = new PopupTab()
                {
                    Left = this.Left + cursor.X,
                    Top = this.Top + cursor.Y
                };
                popupTab.ShowDialog();
                if (popupTab.ToolSelection != null)
                {
                    
                }
            }
        }
        #endregion

        #region Data Binding
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private bool SetField<TType>(ref TType field, TType value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TType>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}