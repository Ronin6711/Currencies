using Currencies.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Currencies.Service
{
    public class GetCurrenciesService : BackgroundService
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://www.cbr-xml-daily.ru/daily_json.js"),
        };

        public GetCurrenciesService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _httpClient.GetAsync("");

                var jsonResponse = await response.Content.ReadAsStringAsync();

                ApiJson? apiJson = JsonSerializer.Deserialize<ApiJson>(jsonResponse);

                _memoryCache.Set("key_currency", apiJson, TimeSpan.FromMinutes(1440));

                Console.WriteLine($"{apiJson}\n");

                await Task.Delay(3600000, stoppingToken);
            }
        }
    }
}
