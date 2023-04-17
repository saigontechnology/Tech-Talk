using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIAppDemo.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace MAUIAppDemo.ViewModels
{

    [QueryProperty("Username", "username")]
    public partial class ChatViewModel : BaseViewModel
    {
        private HubConnection hubConnection;
        private string _userId = Guid.NewGuid().ToString();
        private string _avatar = $"emoji{(new Random().Next(1, 10))}.png";

        [ObservableProperty]
        private ObservableCollection<ChatMessage> _messages;

        [ObservableProperty]
        private string _text;

        [ObservableProperty]
        private string _username;

        public ChatViewModel()
        {
            IsBusy = true;
            Messages = new ObservableCollection<ChatMessage>();

        }

        [RelayCommand]
        public async void SendMessage()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            var message = new ChatMessage()
            {
                UserId = _userId,
                Content = Text,
                SendTime = DateTime.Now,
                Avatar = _avatar,
                UserName = Username
            };
            await hubConnection.InvokeAsync("SendMessageToUser", message);
            Text = string.Empty;
        }

        partial void OnUsernameChanged(string value)
        {
            Username = value;
            UpdateUIWhenUsernameChange(value);
        }

        private void UpdateUIWhenUsernameChange(string value)
        {
            RegisterHub();
        }

        private async void RegisterHub()
        {
            //// because anroid emulator can not connect localhost, so server must connect to real server
            var url = "https://2568-113-160-235-163.ngrok-free.app/chat";
            // var url = "https://localhost:7036/chat";
            hubConnection = new HubConnectionBuilder()
                 .WithUrl(url)
                 .Build();
            await hubConnection.StartAsync();

            await hubConnection.InvokeCoreAsync("PublishUserOnConnect", args: new[] { "Trong" });

            hubConnection.On<ChatMessage>("ReceiveDM", OnReceiveMessage);
            IsBusy = false;
        }

        private void OnReceiveMessage(ChatMessage message)
        {
            message.IsSelfMessage = message.UserId == _userId;
            Messages.Add(message);
        }
    }
}
