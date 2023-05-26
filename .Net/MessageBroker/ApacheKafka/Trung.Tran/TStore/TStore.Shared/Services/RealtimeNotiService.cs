using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TStore.Shared.Models;

namespace TStore.Shared.Services
{
    public interface IRealtimeNotiService
    {
        Task NotifyAsync(NotificationModel notification);
        Task NotifyLogAsync(string id, string logLine);
    }

    public class RealtimeNotiService : IRealtimeNotiService, IDisposable
    {
        private readonly string _realtimeApiUrl;
        private HubConnection _connection;
        private bool _disposedValue;

        public RealtimeNotiService(IConfiguration configuration)
        {
            _realtimeApiUrl = configuration.GetSection("RealtimeApiUrl").Value;
        }

        public async Task NotifyAsync(NotificationModel notification)
        {
            InitConnectionIfNotConnected();

            await _connection.InvokeAsync("Notify", notification);
        }

        public Task NotifyLogAsync(string id, string logLine)
        {
            NotificationModel noti = new NotificationModel
            {
                Data = new LogData { Id = id, LogLine = logLine },
                Type = NotificationModel.NotificationType.Log
            };

            return NotifyAsync(noti);
        }

        private void InitConnectionIfNotConnected()
        {
            lock (this)
            {
                if (_connection == null)
                {
                    _connection = new HubConnectionBuilder()
                        .WithUrl(new Uri(new Uri(_realtimeApiUrl), "/noti"), opt =>
                        {
                            opt.HttpMessageHandlerFactory = (handler) =>
                            {
                                if (handler is HttpClientHandler clientHandler)
                                    // [DEMO] for using self-signed certificate inside Docker
                                    clientHandler.ServerCertificateCustomValidationCallback +=
                                        (sender, certificate, chain, sslPolicyErrors) => true;
                                return handler;
                            };
                        })
                        .WithAutomaticReconnect()
                        .Build();

                    _connection.StartAsync().Wait();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                _connection?.StopAsync().Wait();
                _connection?.DisposeAsync().Wait();

                _disposedValue = true;
            }
        }

        ~RealtimeNotiService()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
