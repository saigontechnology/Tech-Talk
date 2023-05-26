using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TStore.Shared.Configs;
using TStore.Shared.Constants;
using TStore.Shared.Helpers;
using TStore.Shared.Models;
using TStore.Shared.Repositories;

namespace TStore.Shared.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetProductsAsync(SimpleFilterModel filter);
        Task UpdateProductAsync(ProductModel model);
        Task CreateProductAsync(ProductModel model);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IKafkaProducerManager _kafkaProducerManager;
        private readonly IApplicationLog _log;
        private readonly AppProducerConfig _baseConfig;
        private readonly int? _productUpdatedDelay;

        public ProductService(IConfiguration configuration,
            IProductRepository productRepository,
            IKafkaProducerManager kafkaProducerManager,
            IApplicationLog log)
        {
            _productRepository = productRepository;
            _kafkaProducerManager = kafkaProducerManager;
            _log = log;
            _baseConfig = new AppProducerConfig();
            configuration.Bind("CommonProducerConfig", _baseConfig);

            if (configuration.GetValue<bool>("StartFromVS"))
            {
                _baseConfig.FindCertIfNotFound();
            }

            _productUpdatedDelay = configuration.GetValue<int?>("ProductUpdatedDelay");
        }

        public async Task<IEnumerable<ProductModel>> GetProductsAsync(SimpleFilterModel filter)
        {
            IQueryable<Entities.Product> query = _productRepository.Get();

            if (!string.IsNullOrWhiteSpace(filter.Terms))
            {
                query = query.Where(p => p.Name.Contains(filter.Terms));
            }

            if (filter.PageSize != null && filter.Page != null)
            {
                query = query.Skip(filter.Page.Value * filter.PageSize.Value).Take(filter.PageSize.Value);
            }

            ProductModel[] products = await query
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToArrayAsync();

            return products;
        }

        public async Task CreateProductAsync(ProductModel model)
        {
            Entities.Product productEntity = new Entities.Product
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Price = model.Price
            };

            model.Id = productEntity.Id;

            _productRepository.Create(productEntity);

            await _productRepository.UnitOfWork.SaveChangesAsync();

            await PublishProductEventAsync(productEntity, model, EventConstants.Events.ProductCreated);
        }

        public async Task UpdateProductAsync(ProductModel model)
        {
            Entities.Product productEntity = new Entities.Product
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price
            };

            _productRepository.Update(productEntity);

            await _productRepository.UnitOfWork.SaveChangesAsync();

            await PublishProductEventAsync(productEntity, model, EventConstants.Events.ProductUpdated);
        }

        private async Task PublishProductEventAsync(Entities.Product productEntity, ProductModel model, string eventName)
        {
            // [Important] 1 TransactionalId per producer instance
            TransactionalProducerWrapper<string, object> producerWrapper = await _kafkaProducerManager
                .GetTransactionalProducerFromPoolAsync<string, object>(
                    _baseConfig,
                    _baseConfig.DefaultPoolSize,
                    nameof(PublishProductEventAsync),
                    $"_{productEntity.Id}",
                    TimeSpan.FromSeconds(30));

            try
            {
                // [DEMO] heavy producer transaction
                if (_productUpdatedDelay > 0)
                {
                    await Task.Delay(_productUpdatedDelay.Value);
                }

                await producerWrapper.TryRunAsync(async () =>
                {
                    producerWrapper.BeginTransaction();

                    await _log.LogAsync($"Entered transaction {producerWrapper.TransactionalId}");

                    try
                    {
                        await _log.LogAsync($"Producing message {productEntity.Id} in {producerWrapper.TransactionalId}");

                        await producerWrapper.ProduceAsync(eventName,
                            new Confluent.Kafka.Message<string, object>
                            {
                                Key = productEntity.Id.ToString(),
                                Value = model
                            });

                        await _log.LogAsync($"Committing transaction {producerWrapper.TransactionalId}");

                        producerWrapper.CommitTransaction();

                        await _log.LogAsync($"Committed transaction {producerWrapper.TransactionalId}");
                    }
                    catch (Exception ex)
                    {
                        await _log.LogAsync(ex.ToString());

                        producerWrapper.AbortTransaction();

                        await _log.LogAsync($"Aborted transaction {producerWrapper.TransactionalId}");

                        throw ex;
                    }
                });
            }
            finally
            {
                _kafkaProducerManager.Release(producerWrapper);
            }
        }
    }
}
