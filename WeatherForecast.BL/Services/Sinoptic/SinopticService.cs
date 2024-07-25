using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using WeatherForecast.BL.DTO.Requests;
using WeatherForecast.BL.DTO.Response;
using WeatherForecast.BL.Services.Contracts;

namespace WeatherForecast.BL.Services.Sinoptic
{
    /// <summary>
    /// The Sinoptic service implementation.
    /// </summary>
    /// <param name="httpClient">The http client instance.</param>
    /// <param name="logger">The logger instance.</param>
    public class SinopticService(HttpClient httpClient, ILogger<SinopticService> logger) : ISinopticService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<SinopticService> _logger = logger;

        /// <inheritdoc cref="IOpenWeatherAPIService"/>
        public async Task<WeatherForecastResponse?> GetWeatherForecast(WeatherForecastRequest request)
        {
            string url = $"https://ua.sinoptik.ua/погода-{request.City}/{request.Date:yyyy-mm-dd}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);
                var windSpeedNode = htmlDoc.DocumentNode.SelectSingleNode("//td[@class='p6 bR ']/div[contains(@class, 'Tooltip wind')]");

                if (windSpeedNode == null)
                {
                    _logger.LogError("Failed to parse weather data for city: {City}", request.City);
                    return null;
                }

                var weatherForecastResponse = new WeatherForecastResponse()
                {
                    City = request.City,
                    Country = request.Country,
                    Temperature = default(double), // Dynamically loading with JS cause can't be parsed
                    WindSpeed = double.Parse(windSpeedNode.InnerText.Trim().Split(' ')[0]),
                    Date = request.Date,
                    ServiceName = nameof(SinopticService)
                };

                return weatherForecastResponse;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error fetching weather data from HTML page.");
            }
            catch (TaskCanceledException tcEx)
            {
                _logger.LogError(tcEx, "Request to HTML page timed out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching weather data from HTML page.");
            }

            return null;
        }
    }
}
