﻿using Example.Redis.Interfaces;
using Example.Redis.Models;

namespace Example.Redis.Services
{
    public class ProductService : IProductService
    {
        private readonly IRedisService _redisService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRedisService redisService, ILogger<ProductService> logger)
        {
            _redisService = redisService;
            _logger = logger;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("DeleteAsync invoked with parameters: {id}", id);

            var result = await _redisService.RemoveAsync("","",id).ConfigureAwait(false);

            return result > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _redisService.ListAsync<Product>("","").ConfigureAwait(false);
        }

        public Task<Product> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertAsync(Product product)
        {
            return await _redisService.AddAsync("product", product, TimeSpan.FromMinutes(60)).ConfigureAwait(false);
        }

        public Task<bool> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
