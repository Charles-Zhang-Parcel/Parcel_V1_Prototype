namespace Parcel.Shared.Framework
{
    public enum CacheDataType
    {
        // Primitive
        Number,
        String,
        // Basic Numerical
        ParcelDataGrid
    }
    
    public struct ConnectorCacheDescriptor
    {
        public object DataObject { get; set; }
        public CacheDataType DataType { get; set; }

        public bool IsAvailable => DataObject != null;
    }
}