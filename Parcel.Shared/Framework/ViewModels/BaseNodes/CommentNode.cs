using System.Windows;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public class CommentNode: BaseNode
    {
        #region View Components
        private string? _title = "Comment";
        public string? Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }
        
        private string? _comment = string.Empty;
        public string? Comment
        {
            get => _comment;
            set => SetField(ref _comment, value);
        }

        private Size _size;
        public Size Size
        {
            get => _size;
            set => SetField(ref _size, value);
        }
        #endregion
    }
}