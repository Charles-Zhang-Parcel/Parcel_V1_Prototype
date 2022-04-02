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
        
        #region Node Interface
        protected BaseConnector FilePathOutput = new BaseConnector()
        {
            Title = "File"
        }; 
        public OpenFileNode()
        {
            Title = "File";
            Output.Clear();  // TODO: Notice BaseNodes only gets Graph set when Added to Canvas's Nodes property, so it's not available yet here
            Output.Add(FilePathOutput);
        }
        #endregion

        #region Interface
        public override NodeExecutionResult Execute()
        {
            ProcessorCache[FilePathOutput] = new ConnectorCacheDescriptor()
            {
                DataObject = _path,
                DataType = CacheDataType.Number 
            };
            
            return base.Execute();
        }
        #endregion
    }
}