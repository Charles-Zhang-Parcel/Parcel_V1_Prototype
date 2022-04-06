using System;
using System.Collections.Generic;
using Parcel.Shared.DataTypes;

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
        private static readonly Dictionary<Type, CacheDataType> DataTypeMapping = new Dictionary<Type, CacheDataType>()
        {
            {typeof(double), CacheDataType.Number},
            {typeof(int), CacheDataType.Number},
            {typeof(float), CacheDataType.Number},
            {typeof(bool), CacheDataType.Boolean},
            {typeof(string), CacheDataType.Boolean},
            {typeof(DataGrid), CacheDataType.ParcelDataGrid},
        };
        
        public object DataObject { get; set; }
        public CacheDataType DataType { get; set; }

        public ConnectorCacheDescriptor(object dataObject)
        {
            DataObject = dataObject;

            Type type = dataObject.GetType();
            DataType = DataTypeMapping.ContainsKey(type) ? DataTypeMapping[type] : CacheDataType.Generic;   // TODO: or we should potentially throw an error
        }
        public ConnectorCacheDescriptor(object dataObject, CacheDataType dataType)
        {
            DataObject = dataObject;
            DataType = dataType;
        }

        public bool IsAvailable => DataObject != null;
    }
}