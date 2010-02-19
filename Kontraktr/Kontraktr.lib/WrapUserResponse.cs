namespace Kontraktr.lib
{
    public class WrapUserResponse
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Scope { get; set; }

        public string State { get; set; }

        public string ClientId
        {
            get; set;
        }

        public string ClientName
        {
            get;
            set;
        }

        public string CallbackUri
        {
            get;
            set;
        }
    }
}