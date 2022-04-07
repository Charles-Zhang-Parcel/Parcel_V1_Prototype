namespace Parcel.Shared.DataTypes
{
    public struct PresentationLayout
    {
        
    }

    public struct ContentFunction
    {
        
    }

    public struct EndpointDescription
    {
        
    }
    
    public struct ServerConfig
    {
        public ContentFunction Content { get; set; }
        public PresentationLayout Layout { get; set; }
        public EndpointDescription Endpoint { get; set; }
    }
}