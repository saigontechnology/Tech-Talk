using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppShellDemo.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace MauiAppShellDemo.ViewModels
{
    public partial class ChatViewModel : ObservableObject
    {
        private HubConnection hubConnection;

        [ObservableProperty]
        private ObservableCollection<ChatMessage> _messages;

        [ObservableProperty]
        private string _text;

        private string userId = Guid.NewGuid().ToString();

        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            RegisterHub();

        }

        private async void RegisterHub()
        {
            // because of anroid emulator can not connect localhost
            var url = "https://6525-2402-800-6205-60eb-acb2-9b5a-ccc0-1c72.ngrok-free.app/chat";
#if DEBUG
#if ANDROID
            url = "http://10.10.20.61:5002/chat";
#endif
#endif
            hubConnection = new HubConnectionBuilder()
                 .WithUrl(url)
                 .Build();
            await hubConnection.StartAsync();

            // await hubConnection.InvokeCoreAsync("PublishUserOnConnect", args: new[] { "Trong" });

            hubConnection.On<ChatMessage>("ReceiveDM", OnReceiveMessage);
        }

        private void OnReceiveMessage(ChatMessage message)
        {
            message.IsSelfMessage = message.UserId == userId;
            Messages.Add(message);
        }

        [RelayCommand]
        public async void SendMessage()
        {
            var message = new ChatMessage()
            {
                UserId = userId,
                Content = Text,
                SendTime = DateTime.Now,
            };
            //Messages.Add(message);
            Text = string.Empty;
            await hubConnection.InvokeAsync("SendMessageToUser", message);
        }
    }
}
