using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TStore.Shared.Constants;
using TStore.Shared.Models;
using TStore.Shared.Repositories;

namespace TStore.Shared.Services
{
    public interface IInteractionService
    {
        Task SaveInteractionsAsync(List<InteractionModel> interactionModels);
        Task PublishNewUnsavedInteractionAsync(InteractionModel interactionModel);
        Task AggregateInteractionReportAsync(IEnumerable<InteractionModel> newInteractionModels);
        Task AnalyzeInteractionsAsync(IEnumerable<InteractionModel> newInteractionModels);
        Task BigDataLoad(string dest, IEnumerable<InteractionModel> newInteractionModels);
        Task<IEnumerable<InteractionReportModel>> GetInteractionReportsAsync();
    }

    public class InteractionService : IInteractionService
    {
        private static readonly object _reportLock = new object();
        private static readonly List<string> _invalidKeywords
            = new List<string>() { "<", ">", "SELECT", "FROM" };

        private readonly IInteractionRepository _interactionRepository;
        private readonly IInteractionReportRepository _interactionReportRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IRealtimeNotiService _realtimeNotiService;
        private readonly IApplicationLog _log;

        public InteractionService(IInteractionRepository interactionRepository,
            IInteractionReportRepository interactionReportRepository,
            IMessagePublisher messagePublisher,
            IRealtimeNotiService realtimeNotiService,
            IApplicationLog log)
        {
            _interactionRepository = interactionRepository;
            _interactionReportRepository = interactionReportRepository;
            _messagePublisher = messagePublisher;
            _realtimeNotiService = realtimeNotiService;
            _log = log;
        }

        public async Task PublishNewUnsavedInteractionAsync(InteractionModel interactionModel)
        {
            await _messagePublisher.PublishAsync(
                EventConstants.Events.NewUnsavedInteraction,
                Guid.NewGuid().ToString(),
                interactionModel as object, async result =>
                {
                    DeliveryReport<string, object> deliveryReport = result as DeliveryReport<string, object>;

                    if (deliveryReport?.Error?.IsError == true)
                    {
                        await _log.LogAsync($"Produce error: {deliveryReport.Error.Reason}");
                    }
                });
        }

        public async Task SaveInteractionsAsync(List<InteractionModel> interactionModels)
        {
            // [DEMO] heavy task
            await _interactionRepository.ExecuteAsync($@"
DECLARE @sometable table (id uniqueidentifier)

insert into @sometable
SELECT NEWID()
FROM sys.columns A"
            );

            Entities.Interaction[] entities = interactionModels.Select(i => new Entities.Interaction
            {
                Action = i.Action,
                ClickCount = i.ClickCount,
                FromPage = i.FromPage,
                Id = System.Guid.NewGuid(),
                SearchTerm = i.SearchTerm,
                ToPage = i.ToPage,
                UserName = i.UserName,
                Time = DateTimeOffset.UtcNow
            }).ToArray();

            _interactionRepository.Create(entities);

            await _interactionRepository.UnitOfWork.SaveChangesAsync();

            for (int i = 0; i < entities.Length; i++)
            {
                interactionModels[i].Id = entities[i].Id;
                interactionModels[i].Time = entities[i].Time;
            }

            await _messagePublisher.PublishAsync(
                EventConstants.Events.NewRecordedInteraction,
                Guid.NewGuid().ToString(),
                interactionModels as object, async result =>
                {
                    DeliveryReport<string, object> deliveryReport = result as DeliveryReport<string, object>;

                    if (deliveryReport?.Error?.IsError == true)
                    {
                        await _log.LogAsync($"Produce error: {deliveryReport.Error.Reason}");
                    }
                });

            await _realtimeNotiService.NotifyAsync(new NotificationModel
            {
                Data = interactionModels,
                Type = NotificationModel.NotificationType.NewInteractions
            });
        }

        public Task AggregateInteractionReportAsync(IEnumerable<InteractionModel> newInteractionModels)
        {
            IGrouping<Entities.Interaction.ActionType, InteractionModel>[] actionGroups = newInteractionModels.GroupBy(e => e.Action).ToArray();
            Entities.Interaction.ActionType[] actionTypes = actionGroups.Select(g => g.Key).ToArray();

            // [DEMO] should use distributed lock or other mechanism for this
            lock (_reportLock)
            {
                Entities.InteractionReport[] reports = _interactionReportRepository.Get().Where(r => actionTypes.Contains(r.Action)).ToArray();

                foreach (IGrouping<Entities.Interaction.ActionType, InteractionModel> group in actionGroups)
                {
                    Entities.InteractionReport report = reports.FirstOrDefault(r => r.Action == group.Key);

                    bool isNew = false;

                    if (report == null)
                    {
                        isNew = true;
                        report = new Entities.InteractionReport
                        {
                            Action = group.Key,
                            Count = 0,
                            Id = Guid.NewGuid(),
                        };
                    }

                    report.Count += newInteractionModels.Count();

                    if (isNew)
                    {
                        _interactionReportRepository.Create(report);
                    }
                    else
                    {
                        _interactionReportRepository.Update(report);
                    }

                    _interactionReportRepository.UnitOfWork.SaveChangesAsync().Wait();

                    _realtimeNotiService.NotifyAsync(new NotificationModel
                    {
                        Data = new InteractionReportModel
                        {
                            Action = report.Action,
                            Count = report.Count,
                            Id = report.Id
                        },
                        Type = NotificationModel.NotificationType.InteractionReportUpdated
                    }).Wait();
                }
            }

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<InteractionReportModel>> GetInteractionReportsAsync()
        {
            return await _interactionReportRepository.Get().Select(r => new InteractionReportModel
            {
                Action = r.Action,
                Count = r.Count,
                Id = r.Id
            }).ToArrayAsync();
        }

        public async Task AnalyzeInteractionsAsync(IEnumerable<InteractionModel> newInteractionModels)
        {
            UnusualSearchModel[] ununsualSearchs = newInteractionModels
                .Where(i => i.Action == Entities.Interaction.ActionType.Search)
                .Where(i => _invalidKeywords.Any(kw => i.SearchTerm.Contains(kw)))
                .Select(i => new UnusualSearchModel
                {
                    SearchTerm = i.SearchTerm,
                    Time = i.Time,
                    UserName = i.UserName,
                }).ToArray();

            if (ununsualSearchs.Length > 0)
            {
                await _realtimeNotiService.NotifyAsync(new NotificationModel()
                {
                    Data = ununsualSearchs,
                    Type = NotificationModel.NotificationType.UnusualSearchs
                });
            }
        }

        public async Task BigDataLoad(string dest, IEnumerable<InteractionModel> newInteractionModels)
        {
            await Task.Delay(new Random().Next(1000, 2000));

            string[] tsvData = newInteractionModels.Select(i => new[]
            {
                i.Id.ToString(),
                i.UserName,
                i.Action.ToString(),
                i.ClickCount?.ToString(),
                i.FromPage,
                i.ToPage,
                i.Time.ToString(),
                i.SearchTerm
            }.Select(data => data == null ? "" : $"\"{data.Replace("\"", "")}\"").ToArray())
                .Select(data => string.Join('\t', data)).ToArray();

            Directory.CreateDirectory(dest);

            string fileName = $"data_{DateTime.UtcNow:ddMMyyyyHHmmss}_{Guid.NewGuid()}.tsv";

            await File.WriteAllLinesAsync(Path.Combine(dest, fileName), tsvData);
        }
    }
}
