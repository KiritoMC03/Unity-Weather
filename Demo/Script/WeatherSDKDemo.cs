#if UNI_TASK
using System.Threading;
using UnityEngine;
using WeatherSDK.Common.OpenMeteo;
using WeatherSDK.Common.OpenWeather;
using WeatherSDK.Common.Test;
using WeatherSDK.Core;

namespace WeatherSDK.Demo
{
    public class WeatherSDKDemo : MonoBehaviour
    {
        private void Start()
        {
            var provider = new WeatherProvider(new ExampleService());
            var addOpenMeteoResult = provider.AddService(new OpenMeteo());
            if (addOpenMeteoResult.state is AddServiceResultState.Failed)
                Debug.LogWarning($"Cannot add {typeof(OpenMeteo)} service. Reason: {addOpenMeteoResult.failReason}");
                

            // Here is your OpenWeather API key. See https://home.openweathermap.org/api_keys
            var addOpenWeatherResult = provider.AddService(new OpenWeather("12a5029284d4b7234ca5e157ed0781c5"));
            if (addOpenWeatherResult.state is AddServiceResultState.Failed)
                Debug.LogWarning($"Cannot add {typeof(OpenWeather)} service. Reason: {addOpenWeatherResult.failReason}");
            GetWeather(provider);
            GetLocalWeather(provider);
        }

        private async void GetWeather(IWeatherProvider weatherProvider)
        {
            var cts = new CancellationTokenSource();
            // Get weather in London
            var weather = await weatherProvider.GetWeather(latitude: 51.30, longitude: 0.1, cts.Token, timeout: 5f);
            // Weather struct contains list of WeatherInfo from each responding service
            foreach (var info in weather)
                Debug.Log($"London (51.3, 0.1): {info}");
        }

        private async void GetLocalWeather(IWeatherProvider weatherProvider)
        {
            var cts = new CancellationTokenSource();
            // Get weather in current location
            var weather = await weatherProvider.GetWeather(cts.Token, timeout: 5f);
            // Weather struct contains list of WeatherInfo from each responding service
            foreach (var info in weather)
                Debug.Log($"Current Location: {info}");
        }
    }
}
#endif