using Parcel.Shared.Framework.ViewModels;

namespace Parcel.Shared.Serialization
{
    public class Subgraph
    {
        public CanvasSerialization Load(string filePath)
        {
            CanvasSerialization loaded = new GraphSerializer().Deserialize(filePath, new NodesCanvas());
            return loaded;
        }
    }
}