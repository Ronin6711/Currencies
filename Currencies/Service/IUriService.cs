using Currencies.Response.Queries;

namespace Currencies.Service
{
    public interface IUriService
    {
        Uri GetCurrenciesUri(PaginationQuery pagination = null);
    }
}
