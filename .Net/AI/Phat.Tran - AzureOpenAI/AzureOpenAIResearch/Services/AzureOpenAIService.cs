using AzureOpenAIResearch.Models.RequestModels;
using AzureOpenAIResearch.Models.ResponseModels;
using OpenAI.Chat;

namespace AzureOpenAIResearch.Services
{
    public interface IAzureOpenAIService
    {
        Task<SendMessageResponse> SendChatAsync(string prompt, List<MessageModel>? histories);
    }
    public class AzureOpenAIService(ChatClient _chatClient) : IAzureOpenAIService
    {
        public async Task<SendMessageResponse> SendChatAsync(string prompt, List<MessageModel>? histories)
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

                        case ChatMessageRole.Assistant:
                            messages.Add(ChatMessage.CreateAssistantMessage(message.Prompt));
                            break;

                        case ChatMessageRole.Function:
                            messages.Add(ChatMessage.CreateFunctionMessage("Default Function",message.Prompt));
                            break;

                        case ChatMessageRole.Tool:
                            messages.Add(ChatMessage.CreateToolMessage(message.Prompt));
                            break;
                    }
                }
            messages.Add(ChatMessage.CreateUserMessage(prompt));

            var chatCompletion = await _chatClient.CompleteChatAsync(messages);
            var resultValue = chatCompletion.Value;
            return new SendMessageResponse()
            {
                Role = resultValue.Role.ToString(),
                MessageText = resultValue.Content[0].Text,
            };
        }
    }
}
