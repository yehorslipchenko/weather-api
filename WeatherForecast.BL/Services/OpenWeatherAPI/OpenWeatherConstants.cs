namespace WeatherForecast.BL.Services.OpenWeatherAPI
{
    /// <summary>
    /// The OpenWeather constants.
    /// </summary>
    public static class OpenWeatherConstants
    {
        /// <summary>
        /// The API key for accessing the OpenWeather API, stored in environment variables.
        /// </summary>
        public const string API_KEY = "OPENWEATHER_API_KEY";

        /// <summary>
        /// The JSON property name for the latitude in the API response.
        /// </summary>
        public const string LatitudeProperty = "lat";

        /// <summary>
        /// The JSON property name for the longitude in the API response.
        /// </summary>
        public const string LongitudeProperty = "lon";

        /// <summary>
        /// The JSON property name for the timezone in the API response.
        /// </summary>
        public const string TimeZoneProperty = "timezone";

        /// <summary>
        /// Default value to use when a response property is not included in the API response.
        /// </summary>
        public const string ApiResponseNotInclude = "Undefined";

        /// <summary>
        /// The JSON property name for the data in the API response.
        /// </summary>
        public const string DataProperty = "data";

        /// <summary>
        /// The JSON property name for the temperature in the API response.
        /// </summary>
        public const string TemperatureProperty = "temp";

        /// <summary>
        /// The JSON property name for the wind speed in the API response.
        /// </summary>
        public const string WindSpeedProperty = "wind_speed";
    }
}
