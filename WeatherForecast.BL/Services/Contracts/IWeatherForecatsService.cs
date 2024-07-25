using WeatherForecast.BL.DTO.Requests;
using WeatherForecast.BL.DTO.Response;

namespace WeatherForecast.BL.Services.Contracts
{
    /// <summary>
    /// The weather forecast services base contract.
    /// </summary>
    public interface IWeatherForecatsService
    {
        /// <summary>
        /// Retrivies weather forecast.
        /// </summary>
        /// <param name="request">The weather forecast request DTO.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a weather forecast.</returns>
        public Task<WeatherForecastResponse?> GetWeatherForecast(WeatherForecastRequest request);
    }
}
