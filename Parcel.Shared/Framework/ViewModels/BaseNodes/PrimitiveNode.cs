namespace Parcel.Shared.Framework.ViewModels.BaseNodes
{
    public class PrimitiveNode: ProcessorNode
    {
        #region Public View Properties
        private string _value;
        public string Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
        #endregion

        #region Interface
        public PrimitiveNode()
        {
            Output.Add(new BaseConnector()
            {
                Title = "Value"
            });
        }
        #endregion
    }
}