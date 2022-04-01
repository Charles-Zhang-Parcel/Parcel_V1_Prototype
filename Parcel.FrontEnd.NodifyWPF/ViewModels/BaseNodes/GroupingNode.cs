﻿using System.Windows;

namespace Parcel.FrontEnd.NodifyWPF.ViewModels.BaseNodes
{
    public class GroupingNode : BaseNode
    {
        #region View Components
        private string? _title;
        public string? Title
        {
            get => _title;
            set => SetField(ref _title, value);
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