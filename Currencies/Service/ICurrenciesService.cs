using Currencies.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Currencies.Service
{
    public interface ICurrenciesService
    {
        Task<string> GetAll(IMemoryCache memoryCache);

        Task<List<Currency>> GetAllCurrencies(IMemoryCache memoryCache, 
                                              PaginationFilter paginationFilter = null);

        Task<Currency> GetValuteById(IMemoryCache memoryCache, string id);
    }
}
