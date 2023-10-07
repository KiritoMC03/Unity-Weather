#if UNI_TASK
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Common.Utils;
using WeatherSDK.Core;

namespace WeatherSDK.Common.OpenMeteo
{
    public class OpenMeteo : IWeatherService
    {
        private const string WeatherApiUrl = "https://api.open-meteo.com/v1/forecast?";
        private const string Latitude = "latitude=";
        private const string Longitude = "longitude=";
        private const string IsCurrentWeather = "current_weather=";
        private const string And = "&";
        private const string True = "true";

        public async UniTask<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken)
        {
            var requestString = BuildRequestString(coordinates);
            var json = await RequestUtils.SendRequest(requestString, cancellationToken);
            if (string.IsNullOrWhiteSpace(json))
                return WeatherInfo.Empty();

            var weatherForecast = JsonUtility.FromJson<WeatherForecast>(json);
            return new WeatherInfo(
                true, 
                weatherForecast.current_weather.temperature,
                $"{nameof(OpenMeteo)}");
        }

        private static string BuildRequestString(WeatherCoordinates coordinates)
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
                .Append(IsCurrentWeather)
                .Append(True);
            return requestString.ToString();
        }
    }
}
#endif