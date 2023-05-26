using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TStore.Shared.Services
{
    public interface IApplicationLog
    {
        Task LogAsync(string message);
    }

    public class ApplicationLog : IApplicationLog, IDisposable
    {
        private readonly IRealtimeNotiService _realtimeNotiService;
        private readonly Queue<string> _messageQueue;
        private System.Timers.Timer _timer;
        private bool _disposedValue;

        public string Id { get; }

        public ApplicationLog(string id,
            IRealtimeNotiService realtimeNotiService)
        {
            Id = id;
            _messageQueue = new Queue<string>();
            _realtimeNotiService = realtimeNotiService;
        }

        public Task LogAsync(string message)
        {
            InitTimerIfNotAlready();
            Console.WriteLine(message);
            _messageQueue.Enqueue(message);
            return Task.CompletedTask;
        }

        private void InitTimerIfNotAlready()
        {
            lock (this)
            {
                if (_timer == null)
                {
                    _timer = new System.Timers.Timer()
                    {
                        AutoReset = false,
                        Interval = 3000
                    };
                    _timer.Elapsed += async (obj, e) =>
                    {
                        try
                        {
                            _timer.Stop();

                            while (_messageQueue.TryDequeue(out string message))
                            {
                                await _realtimeNotiService.NotifyLogAsync(Id, message);
                            }

                            _timer.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex);
                        }
                        finally
                        {
                            _timer.Start();
                        }
                    };
                    _timer.Start();
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

                _timer?.Stop();
                _timer?.Dispose();

                _disposedValue = true;
            }
        }

        ~ApplicationLog()
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
