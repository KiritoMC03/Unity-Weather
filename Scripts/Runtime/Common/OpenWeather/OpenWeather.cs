#if UNI_TASK
using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Common.Utils;
using WeatherSDK.Core;

namespace WeatherSDK.Common.OpenWeather
{
    public class OpenWeather : IWeatherService
    {
        private const string WeatherApiUrl = "https://api.openweathermap.org/data/2.5/weather?";
        private const string Latitude = "lat=";
        private const string Longitude = "lon=";
        private const string And = "&";
        private const string Units = "units=";
        private const string Metric = "metric";
        private const string AppId = "appid=";

        private readonly string apiKey;

        public OpenWeather(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException($"{nameof(apiKey)} is empty");

            this.apiKey = apiKey;
        }
        
        public async UniTask<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken)
        {
            var requestString = BuildRequestString(coordinates, apiKey);
            var json = await RequestUtils.SendRequest(requestString, cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
                return WeatherInfo.Empty();

            var weatherForecast = JsonUtility.FromJson<WeatherForecast>(json);
            return new WeatherInfo(
                true, 
                weatherForecast.main.temp,
                $"{nameof(OpenWeather)}");
        }

        private static string BuildRequestString(WeatherCoordinates coordinates, string apiKey)
        {
            var requestString = new StringBuilder();
            requestString
                .Append(WeatherApiUrl)
                .Append(Latitude)
                .Append(coordinates.latitude)
                .Append(And)
                .Append(Longitude)
                .Append(coordinates.longitude)
                .Append(And)
                .Append(Units)
                .Append(Metric)
                .Append(And)
                .Append(AppId)
                .Append(apiKey);
            return requestString.ToString();
        }
    }

}
#endif