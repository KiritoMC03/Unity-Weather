using System;

namespace WeatherSDK.Common.OpenMeteo
{
    [Serializable]
    public struct WeatherForecast
    {
        public CurrentWeather current_weather;
    }

    [Serializable]
    public struct CurrentWeather
    {
        public float temperature;
    }
}