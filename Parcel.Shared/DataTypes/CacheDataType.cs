namespace Parcel.Shared.DataTypes
{
    /// <summary>
    /// This will be a subset of <seealso cref="CacheDataType"/>
    /// </summary>
    public enum DictionaryEntryType
    {
        Number,
        String,
        Boolean
    }
    
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
        BatchJob,
        ServerConfig
    }
}