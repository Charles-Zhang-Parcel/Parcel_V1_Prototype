using System;
using Parcel.Shared.Framework.ViewModels.Primitives;

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

    public static class CacheTypeHelper
    {
        public static CacheDataType ConvertToCacheDataType(Type type)
        {
            if (type == typeof(double))
                return DataTypes.CacheDataType.Number;
            throw new ArgumentException();
        }
        public static Type ConvertToNodeType(CacheDataType type)
        {
            switch (type)
            {
                case CacheDataType.Boolean:
                    return typeof(BooleanNode);
                case CacheDataType.Number:
                    return typeof(NumberNode);
                case CacheDataType.String:
                    return typeof(StringNode);
                case CacheDataType.DateTime:
                    return typeof(DateTimeNode);
                case CacheDataType.ParcelDataGrid:
                case CacheDataType.Generic:
                case CacheDataType.BatchJob:
                case CacheDataType.ServerConfig:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static Type ConvertToObjectType(CacheDataType type)
        {
            switch (type)
            {
                case CacheDataType.Boolean:
                    return typeof(bool);
                case CacheDataType.Number:
                    return typeof(double);
                case CacheDataType.String:
                    return typeof(string);
                case CacheDataType.DateTime:
                    return typeof(DateTime);
                case CacheDataType.ParcelDataGrid:
                    return typeof(DataGrid);
                case CacheDataType.Generic:
                case CacheDataType.BatchJob:
                case CacheDataType.ServerConfig:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}