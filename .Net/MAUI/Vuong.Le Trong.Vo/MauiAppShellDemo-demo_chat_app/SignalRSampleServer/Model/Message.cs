namespace SignalRSampleServer.Model
{
    public class Message
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
        public DateTime SendTime { get; set; }
    }
}
