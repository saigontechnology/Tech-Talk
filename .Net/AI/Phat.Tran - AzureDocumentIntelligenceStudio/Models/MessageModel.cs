using OpenAI.Chat;

namespace AzureDocumentIntelligenceStudio.Models
{
    public class MessageModel
    {
        public ChatMessageRole Role { get; set; }
        public string Prompt { get; set; }
    }
}
