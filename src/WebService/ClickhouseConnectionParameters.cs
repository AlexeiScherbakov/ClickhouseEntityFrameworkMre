namespace WebService
{
    public sealed class ClickhouseConnectionParameters
    {
        public string Host { get; set; }

        public ushort Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public static ClickhouseConnectionParameters Default
        {
            get
            {
                return new ClickhouseConnectionParameters()
                {
                    Host = "localhost",
                    Port = 8123,
                    Username = "admin",
                    Password = "password",
                    Database = "default"
                };
            }
        }
    }
}