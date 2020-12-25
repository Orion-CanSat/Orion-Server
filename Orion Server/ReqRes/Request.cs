#nullable enable

namespace OrionServer.ReqRes
{
    public enum RequestType
    {
        GetData,
        InsertData,
        RemoveData,
        InsertKey,
        RemoveKey,
        Authenticate
    }
    public class Request
    {
        public string? requestID = null;
        public string requestData = "";
        public RequestType requestType;
        public string? authenticationKey = null;
    }
}