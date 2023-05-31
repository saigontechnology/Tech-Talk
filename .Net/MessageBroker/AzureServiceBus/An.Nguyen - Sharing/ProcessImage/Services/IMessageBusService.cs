using ProcessImage.Models;
using System.Threading.Tasks;

namespace ProcessImage.Services
{
    public interface IMessageBusService
    {
        Task SendMessageAsync(FileMetadata fileMetadata);
    }
}
