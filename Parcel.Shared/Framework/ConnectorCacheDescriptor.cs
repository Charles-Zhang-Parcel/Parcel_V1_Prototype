namespace Parcel.Shared.Framework
{
    public enum CacheDataType
    {
        // Primitive
        Boolean,
        Number,
        String,
        // Basic Numerical
        ParcelDataGrid,
        // Advanced
        Array,
        Generic,
        BatchJob
    }
    
    public struct ConnectorCacheDescriptor
    {
        public object DataObject { get; set; }
        public CacheDataType DataType { get; set; }

        public bool IsAvailable => DataObject != null;
    }
}