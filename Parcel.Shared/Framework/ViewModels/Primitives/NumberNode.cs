using System.Windows;
using Parcel.Shared.Framework.ViewModels.BaseNodes;

namespace Parcel.Shared.Framework.ViewModels.Primitives
{
    public class NumberNode: PrimitiveNode
    {
        #region View Components
        private double? _number;
        public double? Number
        {
            get => _number;
            set => SetField(ref _number, value);
        }
        #endregion

        public NumberNode()
        {
            Title = "Number";
        }
    }
}