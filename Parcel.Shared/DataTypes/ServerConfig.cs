using System.Collections.Generic;

namespace Parcel.Shared.DataTypes
{
    public enum LayoutElementType
    {
        Page,
        Section,
        Header,
        GridPanel,
        GridLabelElement,
        GridGraph
    }

    public enum ChartType
    {
        Line,
        Bar
    }
    
    public class ServerConfig
    {
        #region Charting
        public ChartType ChartType { get; set; }
        #endregion
        
        #region Content Payload
        public CacheDataType ContentType { get; set; }
        public DataGrid DataGridContent { get; set; }
        public object ObjectContent { get; set; }
        #endregion

        #region Endpoint Configuration
        public string CustomEndpointName { get; set; }
        #endregion

        #region Layout
        public LayoutElementType LayoutSpec { get; set; }
        #endregion

        #region Hierarchy
        public List<ServerConfig> Children { get; set; }
        #endregion
    }
}