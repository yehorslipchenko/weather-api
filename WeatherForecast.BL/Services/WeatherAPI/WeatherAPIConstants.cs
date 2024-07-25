namespace WeatherForecast.BL.Services.WeatherAPI
{
    /// <summary>
    /// The weather API constants.
    /// </summary>
    public static class WeatherAPIConstants
    {
        /// <summary>
        /// The JSON property name for the location data in the API response.
        /// </summary>
        public const string LocationResponseProperty = "location";

        /// <summary>
        /// The JSON property name for the name of the location in the API response.
        /// </summary>
        public const string LocationCityNameResponseProperty = "name";

        /// <summary>
        /// The JSON property name for the name of the location in the API response.
        /// </summary>
        public const string LocationCountryNameResponseProperty = "country";

        /// <summary>
        /// Default value to use when a response property is not included in the API response.
        /// </summary>
        public const string ApiResponseNotInclude = "Undefined";

        /// <summary>
        /// The JSON property name for the forecast data in the API response.
        /// </summary>
        public const string ForecastProperty = "forecast";

        /// <summary>
        /// The JSON property name for the forecast day data in the API response.
        /// </summary>
        public const string ForecastDayProperty = "forecastday";

        /// <summary>
        /// The JSON property name for the day data within a forecast day in the API response.
        /// </summary>
        public const string DayProperty = "day";

        /// <summary>
        /// The JSON property name for the date of the forecast day in the API response.
        /// </summary>
        public const string DateProperty = "date";

        /// <summary>
        /// The JSON property name for the average temperature in the API response.
        /// </summary>
        public const string AverageTempProperty = "avgtemp_c";

        /// <summary>
        /// The JSON property name for the maximum wind speed in the API response.
        /// </summary>
        public const string MaxWindSpeedProperty = "maxwind_kph";

        /// <summary>
        /// The API key for accessing the Weather API, stored in environment variables.
        /// </summary>
        public const string API_KEY = "WEATHERAPI_API_KEY";

        /// <summary>
        /// The convertation factor (km per hour to m/s)
        /// </summary>
        public const double ConvertationFactor = 3.6;
    }
}
