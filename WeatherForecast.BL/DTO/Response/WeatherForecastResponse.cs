namespace WeatherForecast.BL.DTO.Response
{
    public class WeatherForecastResponse
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

        /// <summary>
        /// Forecast temperature (measured in degrees Celsius).
        /// </summary>
        public required double Temperature { get; set; }

        /// <summary>
        /// Forecast wind (measured in kilometers per hour)
        /// </summary>
        public required double WindSpeed { get; set; }

        public required string ServiceName { get; set; }
    }
}
