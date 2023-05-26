using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TStore.Shared.Constants;
using TStore.Shared.Helpers;
using TStore.Shared.Services;
using TStore.SystemApi.Configs;
using TStore.SystemApi.Models;

namespace TStore.SystemApi.Services
{
    public interface IMessageBrokerService
    {
        Task InitializeTopicsAsync();
        Task InitializeAclsAsync();
        Task ClearRecordsAsync(string topic);
        Task CreateTombstoneAsync(TombstoneModel model);
        Task DeleteGroupAsync(string groupId);
    }

    public class KafkaMessageBrokerService : IMessageBrokerService, IDisposable
    {
        private bool _disposedValue;
        private readonly IAdminClient _adminClient;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IConsumer<string, string> _offsetConsumer;
        private readonly IApplicationLog _log;
        private readonly AdminClientConfig _adminConfig;
        private readonly TopicsConfigurations _topicsConfigs;

        public KafkaMessageBrokerService(IConfiguration configuration,
            IApplicationLog log,
            IMessagePublisher messagePublisher)
        {
            _log = log;
            _messagePublisher = messagePublisher;

            _topicsConfigs = new TopicsConfigurations();
            configuration.GetSection("TopicsConfigurations").Bind(_topicsConfigs);

            _adminConfig = new AdminClientConfig();
            configuration.Bind("CommonAdminClientConfig", _adminConfig);

            if (configuration.GetValue<bool>("StartFromVS"))
            {
                _adminConfig.FindCertIfNotFound();
            }

            _adminClient = new AdminClientBuilder(_adminConfig).Build();

            ConsumerConfig consumerConfig = new ConsumerConfig(_adminConfig);
            configuration.Bind("OffsetConsumerConfig", consumerConfig);
            _offsetConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public async Task InitializeTopicsAsync()
        {
            Metadata metadata = _adminClient.GetMetadata(TimeSpan.FromSeconds(10));

            string[] topicNames = metadata.Topics.Select(t => t.Topic).ToArray();

            List<TopicSpecification> topicSpecs = new List<TopicSpecification>();

            if (!topicNames.Contains(EventConstants.Events.NewOrder))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.NewOrder,
                    NumPartitions = 7,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.PromotionApplied))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.PromotionApplied,
                    NumPartitions = 7,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.ShipApplied))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.ShipApplied,
                    NumPartitions = 7,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.ProductUpdated))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.ProductUpdated,
                    NumPartitions = 1,
                    Configs = _topicsConfigs.ProductTopics,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.ProductCreated))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.ProductCreated,
                    NumPartitions = 2,
                    Configs = _topicsConfigs.ProductTopics,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.NewUnsavedInteraction))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.NewUnsavedInteraction,
                    NumPartitions = 7,
                    Configs = _topicsConfigs.Interaction,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.NewRecordedInteraction))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.NewRecordedInteraction,
                    NumPartitions = 7,
                    Configs = _topicsConfigs.Interaction,
                    ReplicationFactor = 3
                });
            }

            if (!topicNames.Contains(EventConstants.Events.SampleEvents))
            {
                topicSpecs.Add(new TopicSpecification
                {
                    Name = EventConstants.Events.SampleEvents,
                    NumPartitions = 1,
                    ReplicationFactor = 3
                });
            }

            if (topicSpecs.Count > 0)
            {
                await _adminClient.CreateTopicsAsync(topicSpecs);

                await _log.LogAsync($"Finish creating topics: " +
                    $"{string.Join(", ", topicSpecs.Select(spec => spec.Name))}");
            }
        }

        public async Task InitializeAclsAsync()
        {
            Metadata metadata = _adminClient.GetMetadata(TimeSpan.FromSeconds(10));

            DescribeAclsResult describeResult = await _adminClient.DescribeAclsAsync(new AclBindingFilter()
            {
                PatternFilter = new ResourcePatternFilter
                {
                    ResourcePatternType = ResourcePatternType.Any,
                    Type = ResourceType.Any,
                },
                EntryFilter = new AccessControlEntryFilter
                {
                    Operation = AclOperation.Any,
                    PermissionType = AclPermissionType.Any,
                }
            });

            AccessControlEntry allowConsumerRead = new AccessControlEntry
            {
                Host = "*",
                Operation = AclOperation.Read,
                PermissionType = AclPermissionType.Allow,
                Principal = "User:consumer"
            };

            AccessControlEntry allowTransactionalProducerRead = new AccessControlEntry
            {
                Host = "*",
                Operation = AclOperation.Read,
                PermissionType = AclPermissionType.Allow,
                Principal = "User:transproducer"
            };

            List<AclBinding> aclBindings = new List<AclBinding>()
            {
                // Producer ACL
                new AclBinding
                {
                    Entry = new AccessControlEntry
                    {
                        Host = "*",
                        Operation = AclOperation.Write,
                        PermissionType = AclPermissionType.Allow,
                        Principal = "User:producer"
                    },
                    Pattern = new ResourcePattern
                    {
                        Name = "*",
                        Type = ResourceType.Topic,
                        ResourcePatternType = ResourcePatternType.Literal
                    }
                },

                // Transactional Producer ACL
                new AclBinding
                {
                    Entry = new AccessControlEntry
                    {
                        Host = "*",
                        Operation = AclOperation.Write,
                        PermissionType = AclPermissionType.Allow,
                        Principal = "User:transproducer"
                    },
                    Pattern = new ResourcePattern
                    {
                        Name = "*",
                        Type = ResourceType.Topic,
                        ResourcePatternType = ResourcePatternType.Literal
                    }
                },
                new AclBinding
                {
                    Entry = allowTransactionalProducerRead,
                    Pattern = new ResourcePattern
                    {
                        Name = "*",
                        Type = ResourceType.Group,
                        ResourcePatternType = ResourcePatternType.Literal
                    }
                },
                new AclBinding
                {
                    Entry = allowTransactionalProducerRead,
                    Pattern = new ResourcePattern
                    {
                        Name = "*",
                        Type = ResourceType.Topic,
                        ResourcePatternType = ResourcePatternType.Literal
                    }
                },

                // Consumer ACL
                new AclBinding
                {
                    Entry = allowConsumerRead,
                    Pattern = new ResourcePattern
                    {
                        Name = "*",
                        Type = ResourceType.Topic,
                        ResourcePatternType = ResourcePatternType.Literal
                    }
                },
                new AclBinding
                {
                    Entry = allowConsumerRead,
                    Pattern = new ResourcePattern
                    {
                        Name = "*",
                        Type = ResourceType.Group,
                        ResourcePatternType = ResourcePatternType.Literal
                    }
                }
            };

            aclBindings = aclBindings
                .Where(srcBinding => !describeResult.AclBindings.Any(destBinding => AclEquals(srcBinding, destBinding)))
                .ToList();

            if (aclBindings.Count > 0)
            {
                Console.WriteLine("Creating ACLs:");
                foreach (AclBinding aclBinding in aclBindings)
                {
                    Console.WriteLine(aclBinding);
                }

                await _adminClient.CreateAclsAsync(aclBindings);
            }
        }

        private bool AclEquals(AclBinding src, AclBinding dest)
        {
            return src.Entry?.Principal == dest.Entry?.Principal
                && src.Entry?.PermissionType == dest.Entry?.PermissionType
                && src.Entry?.Operation == dest.Entry?.Operation
                && src.Entry?.Host == dest.Entry?.Host
                && src.Pattern?.ResourcePatternType == dest.Pattern?.ResourcePatternType
                && src.Pattern?.Type == dest.Pattern?.Type
                && src.Pattern?.Name == dest.Pattern?.Name;
        }

        public async Task ClearRecordsAsync(string topic)
        {
            Metadata metadata = _adminClient.GetMetadata(TimeSpan.FromSeconds(30));
            TopicMetadata topicConfig = metadata.Topics.FirstOrDefault(t => t.Topic == topic);

            if (topicConfig == null)
            {
                throw new Exception("Topic not found");
            }

            TopicPartitionOffset[] partitionOffsets = topicConfig.Partitions.Select(p =>
            {
                Partition partition = new Partition(p.PartitionId);
                TopicPartition topicPartition = new TopicPartition(topic, partition);
                WatermarkOffsets watermarksOffset = GetLatestOffset(topicPartition);
                _log.LogAsync($"Partition {p.PartitionId}: {watermarksOffset.Low} - {watermarksOffset.High}").Wait();
                Offset offset = new Offset(-1);
                return new TopicPartitionOffset(topicPartition, offset);
            }).ToArray();

            List<DeleteRecordsResult> results = await _adminClient.DeleteRecordsAsync(partitionOffsets);

            foreach (DeleteRecordsResult deletedPartition in results)
            {
                await _log.LogAsync($"Deleted all records of partition {deletedPartition.Partition.Value} of topic {topic}");
            }
        }

        public async Task CreateTombstoneAsync(TombstoneModel model)
        {
            // [DEMO] create new tombstone (delete marker) for compacted topic
            await _messagePublisher.PublishAndWaitAsync<string, Null>(model.Topic, model.Key, null);
        }

        public async Task DeleteGroupAsync(string groupId)
        {
            await _adminClient.DeleteGroupsAsync(new[] { groupId });
        }

        private WatermarkOffsets GetLatestOffset(TopicPartition topicPartition)
        {
            return _offsetConsumer.QueryWatermarkOffsets(topicPartition, TimeSpan.FromSeconds(30));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _adminClient?.Dispose();
                    _offsetConsumer?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TopicManagementService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
