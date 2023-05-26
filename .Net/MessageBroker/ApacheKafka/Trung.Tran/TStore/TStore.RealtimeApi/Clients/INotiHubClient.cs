using System.Threading.Tasks;
using TStore.Shared.Models;

namespace TStore.RealtimeApi.Clients
{
    public interface INotiHubClient
    {
        Task HandleNotification(NotificationModel notificationModel);
    }
}
