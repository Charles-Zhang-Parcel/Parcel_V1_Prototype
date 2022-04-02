namespace Parcel.Shared.Framework
{
    public struct NodeExecutionResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public NodeExecutionResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}