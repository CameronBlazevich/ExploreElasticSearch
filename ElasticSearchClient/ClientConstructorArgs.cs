namespace ElasticSearchClient
{
    public class ClientConstructorArgs
    {
        public ClientConstructorArgs(string url, string username, string password)
        {
            Url = url;
            Username = username;
            Password = password;
        }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}