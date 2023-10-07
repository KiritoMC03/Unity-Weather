using System;

namespace WeatherSDK.Common.OpenWeather
{
    [Serializable]
    public struct WeatherForecast
    {
        public MainWeather main;
    }

    [Serializable]
    public struct MainWeather
    {
        public float temp;
    }
}