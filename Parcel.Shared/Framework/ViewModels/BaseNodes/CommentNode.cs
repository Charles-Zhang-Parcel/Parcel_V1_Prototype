using System;
using System.Collections.Generic;
using System.Windows;

namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public class CommentNode: BaseNode
    {
        #region Construction
        public CommentNode()
        {
            MemberSerialization = new List<NodeSerializationRoutine>()
            {
                new NodeSerializationRoutine("Comment", () => _comment, value => _comment = value as string)
            };
        }
        #endregion
        
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

        #region Serialization
        public sealed override List<NodeSerializationRoutine> MemberSerialization { get; set; }
        public override int GetOutputPinID(BaseConnector connector) => throw new InvalidOperationException();
        public override int GetInputPinID(BaseConnector connector) => throw new InvalidOperationException();
        public override BaseConnector GetOutputPin(int id) => throw new InvalidOperationException();
        public override BaseConnector GetInputPin(int id) => throw new InvalidOperationException();
        #endregion
    }
}