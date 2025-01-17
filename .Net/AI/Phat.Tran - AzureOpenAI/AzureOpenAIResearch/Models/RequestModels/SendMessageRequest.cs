using OpenAI.Chat;

namespace AzureOpenAIResearch.Models.RequestModels
{
    public class SendMessageRequest
    {
        public string Prompt { get; set; }
        public List<MessageModel>? Histories { get; set; }
    }
    public class MessageModel
    {
        public ChatMessageRole Role { get; set; }
        public string Prompt { get; set; }
    }
}
