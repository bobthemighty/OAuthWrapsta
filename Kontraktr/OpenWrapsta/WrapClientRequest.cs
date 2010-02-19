namespace OpenWrapsta
{
    public class WrapClientRequest
    {
        private readonly string _clientId;
        private readonly string _clientName;
        private readonly string _callbackUri;

        public WrapClientRequest(string clientId, string clientName, string callbackUri)
        {
            _clientId = clientId;
            _clientName = clientName;
            _callbackUri = callbackUri;
        }

        public string Scope { get; set; }

        public string State { get; set; }

        public string ClientId
        {
            get { return _clientId; }
        }

        public string ClientName
        {
            get { return _clientName; }
        }

        public string CallbackUri
        {
            get { return _callbackUri; }
        }
    }
}
