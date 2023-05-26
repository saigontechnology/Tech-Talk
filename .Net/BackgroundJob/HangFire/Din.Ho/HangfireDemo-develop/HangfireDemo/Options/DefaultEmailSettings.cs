namespace HangfireDemo.Options
{
    public class DefaultEmailSettings
    {
        public const string Sender = nameof(Sender);
        public const string Recipient = nameof(Recipient);

        public string Name { get; set; }
        public string Email { get; set; }
    }
}
