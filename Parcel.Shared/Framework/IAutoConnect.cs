using System;
using Parcel.Shared.DataTypes;
using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Framework
{
    public interface IAutoConnect
    {
        public bool ShouldHaveAutoConnection { get; }
        public Tuple<ToolboxNodeExport, Vector2D, InputConnector>[] AutoGenerateNodes { get; }
    }
}