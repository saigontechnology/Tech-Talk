using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TStore.RealtimeApi.Clients;
using TStore.Shared.Models;

namespace TStore.RealtimeApi.Hubs
{
    public class NotificationHub : Hub<INotiHubClient>
    {
        public async Task Notify(NotificationModel notificationModel)
        {
            await Clients.Others.HandleNotification(notificationModel);
        }
    }
}
