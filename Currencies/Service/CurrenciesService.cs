using Currencies.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Currencies.Service
{
    public class CurrenciesService : ICurrenciesService
    {
        public async Task<string> GetAll(IMemoryCache memoryCache)
        {
            ApiJson apiJson;

            if(!memoryCache.TryGetValue("key_currency", out apiJson))
            {
                return null;
            }

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            return JsonSerializer.Serialize(apiJson, options);
        }

        public async Task<List<Currency>> GetAllCurrencies(IMemoryCache memoryCache, PaginationFilter paginationFilter = null)
        {
            ApiJson apiJson;
            if (!memoryCache.TryGetValue("key_currency", out apiJson))
            {
                return null;
            }

            if (paginationFilter == null)
            {
                return apiJson.Valute.Values.ToList();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            return apiJson.Valute.Values.Skip(skip).Take(paginationFilter.PageSize).ToList();
        }

        public async Task<Currency> GetValuteById(IMemoryCache memoryCache, string id)
        {
            ApiJson apiJson;
            if (!memoryCache.TryGetValue("key_currency", out apiJson))
            {
                return null;
            }

            var result = apiJson.Valute.Values.FirstOrDefault(x => x.ID == id);

            return result;
        }
    }
}
