using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WeatherForecast.BL.DTO.Requests;
using WeatherForecast.BL.DTO.Response;
using WeatherForecast.BL.Services.Contracts;

namespace WeatherForecast.BL.Services.OpenWeatherAPI
{
    /// <summary>
    /// The OpenWeatherAPI service implementation.
    /// </summary>
    /// <param name="httpClient">The http client instance.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="configuration">The configuration instance.</param>
    public class OpenWeatherAPIService(HttpClient httpClient, ILogger<OpenWeatherAPIService> logger, IConfiguration configuration) : IOpenWeatherAPIService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<OpenWeatherAPIService> _logger = logger;
        private readonly string _apiKey = configuration[OpenWeatherConstants.API_KEY]
                ?? throw new Exception("The weather API api_key not found.");

        /// <inheritdoc cref="IOpenWeatherAPIService"/>
        public async Task<WeatherForecastResponse?> GetWeatherForecast(WeatherForecastRequest request)
        {
            var coordinates = await GetCityCoordinates(request.City);
            if (coordinates == null)
            {
                _logger.LogError("Coordinates not found for city: {City}", request.City);
                return null;
            }

            long unixTimestamp = ((DateTimeOffset)request.Date).ToUnixTimeSeconds();
            string url = $"https://api.openweathermap.org/data/3.0/onecall/timemachine?lat={coordinates.Item1}&lon={coordinates.Item2}&dt={unixTimestamp}&appid={_apiKey}&units=metric";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                JObject weatherData = JObject.Parse(responseData);

                var weatherForecastResponse = new WeatherForecastResponse()
                {
                    City = request.City,
                    Country = (string?)weatherData[OpenWeatherConstants.TimeZoneProperty] ?? OpenWeatherConstants.ApiResponseNotInclude,
                    Temperature = (double?)weatherData[OpenWeatherConstants.DataProperty]?[0]?[OpenWeatherConstants.TemperatureProperty] ?? double.NaN,
                    WindSpeed = (double?)weatherData[OpenWeatherConstants.DataProperty]?[0]?[OpenWeatherConstants.WindSpeedProperty] ?? double.NaN,
                    Date = request.Date,
                    ServiceName = nameof(OpenWeatherAPIService)
                };

                return weatherForecastResponse;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error fetching weather data from OpenWeatherMap.");
            }
            catch (TaskCanceledException tcEx)
            {
                _logger.LogError(tcEx, "Request to OpenWeatherMap timed out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching weather data from OpenWeatherMap.");
            }

            return null;
        }

        private async Task<Tuple<double?, double?>?> GetCityCoordinates(string city)
        {
            string geocodingUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={_apiKey}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(geocodingUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                JArray geoData = JArray.Parse(responseData);

                if (geoData.Count == 0)
                {
                    return null;
                }

                var lat = (double?)geoData?[0]?[OpenWeatherConstants.LatitudeProperty];
                var lon = (double?)geoData?[0]?[OpenWeatherConstants.LongitudeProperty];

                return new Tuple<double?, double?>(lat, lon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching coordinates for city: {City}", city);
                return null;
            }
        }
    }
}
