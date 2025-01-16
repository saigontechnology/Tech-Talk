namespace AzureDocumentIntelligenceStudio.Models.RequestModels
{
    public class ChatRequest
    {
        public string Prompt { get; set; }
        public List<MessageModel>? Histories { get; set; }
    }
}
