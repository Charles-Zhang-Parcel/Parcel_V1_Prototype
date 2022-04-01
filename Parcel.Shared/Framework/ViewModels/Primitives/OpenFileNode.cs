using System.IO;
using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class OpenFileNode: StringNode
    {
        #region View Components
        private string? _path;
        public string? Path
        {
            get => _path;
            set => SetField(ref _path, File.Exists(value) ? value : null);
        }
        #endregion
        
        public OpenFileNode()
        {
            Title = "File";
        }
    }
}