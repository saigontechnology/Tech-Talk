using Microsoft.AspNetCore.SignalR;
using SignalRSampleServer.Model;

namespace SignalRSampleServer.Hubs
{
    public class ChatHub : Hub
    {

        public Task SendMessageToUser(Message message)
        {
            return Clients.All.SendAsync("ReceiveDM", message);
        }

        public async Task PublishUserOnConnect(string username)
        {
            await Clients.All.SendAsync("BroadcastUserOnConnect", username);

        }
    }
}
