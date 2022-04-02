using System.Windows.Media.Imaging;

namespace Parcel.Shared.Framework
{
    public enum CacheDataType
    {
        // Primitive
        Number,
        String,
        // Basic Numerical
        Table
    }
    
    public struct ConnectorCacheDescriptor
    {
        public object DataObject { get; set; }
        public CachedBitmap DataType { get; set; }

        public bool IsAvailable => DataObject != null;
    }
}