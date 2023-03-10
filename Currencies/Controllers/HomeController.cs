using Currencies.Models;
using Currencies.Response;
using Currencies.Response.Queries;
using Currencies.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Currencies.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ICurrenciesService _currenciesService;
        private readonly IUriService _uriService;

        public HomeController(IMemoryCache cache, ICurrenciesService currenciesService, IUriService uriService)
        {
            _cache = cache;
            _currenciesService = currenciesService;
            _uriService = uriService;
        }

        /// <summary>
        /// Возвращает json информацию о курсе
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/all")]
        public async Task<IActionResult> GetAllInfo()
        {
            var apiJson = await _currenciesService.GetAll(_cache);

            if(apiJson == null)
            {
                return NotFound();
            }

            return Ok(apiJson);
        }

        /// <summary>
        /// Djpdhfoftn только курс валют с пагинацией
        /// </summary>
        /// <param name="paginationQuery"></param>
        /// <returns></returns>
        [HttpGet("/api/currencies")]
        public async Task<IActionResult> GetAllCurrencies([FromQuery]PaginationQuery paginationQuery)
        {
            var paginationFilter = new PaginationFilter
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize
            };

            var apiJson = await _currenciesService.GetAllCurrencies(_cache, paginationFilter);

            if (apiJson == null)
            {
                return NotFound();
            }

            var response = apiJson.Select(currencies => new ValuteResponse
            {
                ID = currencies.ID,
                NumCode = currencies.NumCode,
                CharCode = currencies.NumCode,
                Nominal = currencies.Nominal,
                Name = currencies.Name,
                Value = currencies.Value,
                Previous = currencies.Previous
            });

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<ValuteResponse>(response));
            }

            var nextPage = paginationFilter.PageNumber >= 1 ? _uriService
                .GetCurrenciesUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
                : null;

            var previousPage = paginationFilter.PageNumber - 1 >= 1 ? _uriService
                .GetCurrenciesUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
                : null;
            var paginationResponse = new PagedResponse<ValuteResponse>
            {
                Data = response,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage
            };

            return Ok(paginationResponse);
        }
        
        /// <summary>
        /// Возвращает курс валюты по ID.
        /// </summary>
        /// <param name="valuteId"></param>
        /// <returns></returns>
        [HttpGet("/api/{valuteId}")]
        public async Task<IActionResult> GetValute([FromRoute] string valuteId)
        {
            var valute = await _currenciesService.GetValuteById(_cache, valuteId);

            if (valute == null)
            {
                return NotFound();
            }

            var response = new ValuteResponse
            {
                ID = valute.ID,
                NumCode = valute.NumCode,
                CharCode = valute.CharCode,
                Nominal = valute.Nominal,
                Name = valute.Name,
                Value = valute.Value,
                Previous = valute.Previous
            };

            return Ok(new Response<ValuteResponse>(response));
        }
    }
}