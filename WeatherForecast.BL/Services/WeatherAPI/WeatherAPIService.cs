using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WeatherForecast.BL.DTO.Requests;
using WeatherForecast.BL.DTO.Response;
using WeatherForecast.BL.Services.Contracts;

namespace WeatherForecast.BL.Services.WeatherAPI
{
    /// <summary>
    /// The weather API service implementation.
    /// </summary>
    /// <param name="httpClient">The http client instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The configuration instance.</param>
    public class WeatherAPIService(HttpClient httpClient, ILogger<WeatherAPIService> logger, IConfiguration configuration) : IWeatherAPIService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<WeatherAPIService> _logger = logger;
        private readonly string _apiKey = configuration[WeatherAPIConstants.API_KEY]
                ?? throw new Exception("The weather API api_key not found.");

        /// <inheritdoc cref="IWeatherAPIService"/>
        public async Task<WeatherForecastResponse?> GetWeatherForecast(WeatherForecastRequest request)
        {
            string url = $"http://api.weatherapi.com/v1/history.json?key={_apiKey}&q={request.City}&dt={request.Date:yyyy-MM-dd}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                JObject weatherData = JObject.Parse(responseData);

                var weatherForecastResponse = new WeatherForecastResponse()
                {
                    City = (string?)weatherData[WeatherAPIConstants.LocationResponseProperty]?[WeatherAPIConstants.LocationCityNameResponseProperty] 
                                    ?? WeatherAPIConstants.ApiResponseNotInclude,

                    Country = (string?)weatherData[WeatherAPIConstants.LocationResponseProperty]?[WeatherAPIConstants.LocationCountryNameResponseProperty] 
                                    ?? WeatherAPIConstants.ApiResponseNotInclude,

                    Temperature = (double?)weatherData[WeatherAPIConstants.ForecastProperty]?[WeatherAPIConstants.ForecastDayProperty]?[0]?[WeatherAPIConstants.DayProperty]?[WeatherAPIConstants.AverageTempProperty] 
                                    ?? double.NaN,

                    WindSpeed = (double?)weatherData[WeatherAPIConstants.ForecastProperty]?[WeatherAPIConstants.ForecastDayProperty]?[0]?[WeatherAPIConstants.DayProperty]?[WeatherAPIConstants.MaxWindSpeedProperty] 
                                    ?? double.NaN,

                    Date = (DateTime?)weatherData[WeatherAPIConstants.ForecastProperty]?[WeatherAPIConstants.ForecastDayProperty]?[0]?[WeatherAPIConstants.DateProperty] 
                                    ?? default,

                    ServiceName = nameof(WeatherAPIService)
                };

                if(weatherForecastResponse.WindSpeed != double.NaN)
                {
                    weatherForecastResponse.WindSpeed /= WeatherAPIConstants.ConvertationFactor;
                }

                return weatherForecastResponse;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error fetching weather data from WeatherAPI.");
            }
            catch (TaskCanceledException tcEx)
            {
                _logger.LogError(tcEx, "Request to WeatherAPI timed out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching weather data from WeatherAPI.");
            }

            return null;
        }
    }
}
