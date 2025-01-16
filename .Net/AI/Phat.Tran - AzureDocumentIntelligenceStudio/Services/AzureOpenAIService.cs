using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using AzureDocumentIntelligenceStudio.Models;
using AzureDocumentIntelligenceStudio.Models.ResonseModel;
using OpenAI.Chat;

namespace AzureDocumentIntelligenceStudio.Services
{
    public interface IAzureOpenAIService
    {
        Task<ChatResponse> SendChatAsync(string prompt, List<MessageModel>? histories);
    }
    public class AzureOpenAIService(ChatClient _chatClient) : IAzureOpenAIService
    {
        public async Task<ChatResponse> SendChatAsync(string prompt, List<MessageModel>? histories)
        {
            var messages = new List<ChatMessage>();
            if (histories != null && histories.Any())
                foreach (var message in histories)
                {
                    switch (message.Role)
                    {
                        case ChatMessageRole.User:
                            messages.Add(ChatMessage.CreateUserMessage(message.Prompt));
                            break;

                        case ChatMessageRole.System:
                            messages.Add(ChatMessage.CreateSystemMessage(message.Prompt));
                            break;
                    }
                }
            messages.Add(ChatMessage.CreateUserMessage(prompt));

            var chatCompletion = await _chatClient.CompleteChatAsync(messages);
            var resultValue = chatCompletion.Value;
            return new ChatResponse()
            {
                Role = resultValue.Role.ToString(),
                MessageText = resultValue.Content[0].Text,
            };
        }
    }
}
