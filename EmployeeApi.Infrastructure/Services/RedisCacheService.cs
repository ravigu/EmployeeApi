using EmployeeApi.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeApi.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(
            IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value == null)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>( string key,T value, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var json = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(
                key,
                json,
                options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
    }
