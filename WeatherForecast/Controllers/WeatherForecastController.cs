using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WeatherForecast.BL.DTO.Requests;
using WeatherForecast.BL.DTO.Response;
using WeatherForecast.BL.Services.Contracts;

namespace WeatherForecast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherAPIService _weatherAPIService;
        private readonly IOpenWeatherAPIService _openWeatherAPIService;
        private readonly ISinopticService _sinopticService;
        private readonly IMemoryCache _memoryCache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                         IWeatherAPIService weatherAPIService,
                                         IOpenWeatherAPIService openWeatherAPIService,
                                         ISinopticService sinopticService,
                                         IMemoryCache memoryCache)
        {
            _logger = logger;
            _weatherAPIService = weatherAPIService;
            _openWeatherAPIService = openWeatherAPIService;
            _sinopticService = sinopticService;
            _memoryCache = memoryCache;
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<WeatherForecastResponse>))]
        public async Task<ActionResult> Get([FromQuery] WeatherForecastRequest request)
        {
            try
            {
                _logger.LogInformation("Received request to retrieve a weather forecast");

                string cacheKey = $"WeatherForecast_{request.City}_{request.Date}";
                if (!_memoryCache.TryGetValue(cacheKey, out List<WeatherForecastResponse> weatherForecast))
                {
                    _logger.LogInformation("Cache miss. Retrieving from services.");
                    var weatherForecastArray = await Task.WhenAll(_weatherAPIService.GetWeatherForecast(request),
                                                                  _openWeatherAPIService.GetWeatherForecast(request),
                                                                  _sinopticService.GetWeatherForecast(request));

                    weatherForecast = weatherForecastArray.ToList();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _memoryCache.Set(cacheKey, weatherForecast, cacheEntryOptions);
                }
                else
                {
                    _logger.LogInformation("Cache hit. Returning cached data.");
                }

                return Ok(weatherForecast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving weather forecast.");
                return BadRequest(ex.Message);
            }
        }
    }
}