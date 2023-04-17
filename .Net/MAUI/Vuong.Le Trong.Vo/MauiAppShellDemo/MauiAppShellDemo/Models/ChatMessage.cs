namespace MauiAppShellDemo.Models
{
    public class ChatMessage
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
        public bool IsSelfMessage { get; set; }
    }
}
