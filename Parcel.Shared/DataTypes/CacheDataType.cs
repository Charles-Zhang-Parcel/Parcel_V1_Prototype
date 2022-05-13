using System;

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
        DateTime,
        // Basic Numerical
        ParcelDataGrid, // Including arrays
        // Advanced
        Generic,
        BatchJob,
        ServerConfig
    }
}