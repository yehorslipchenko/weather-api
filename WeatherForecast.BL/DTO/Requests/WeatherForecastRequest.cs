namespace WeatherForecast.BL.DTO.Requests
{
    /// <summary>
    /// The weather forecast request DTO.
    /// </summary>
    public class WeatherForecastRequest
    {
        /// <summary>
        /// The date of weather forecast.
        /// </summary>
        public required DateTime Date { get; set; }

        /// <summary>
        /// The city of weather forecast.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// The country of weather forecast.
        /// </summary>
        public required string Country { get; set; }
    }
}
