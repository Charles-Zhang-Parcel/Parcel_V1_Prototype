using System;
using System.Windows;
using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public interface IAutoConnect
    {
        public bool ShouldHaveAutoConnection { get; }
        public Tuple<ToolboxNodeExport, Vector, InputConnector>[] AutoGenerateNodes { get; }
    }
}