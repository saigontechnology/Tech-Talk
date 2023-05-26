namespace HangfireDemo.Options
{
    public class EmailServerSettings
    {
        public const string EmailServer = nameof(EmailServer);

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
