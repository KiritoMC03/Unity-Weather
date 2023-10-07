#if UNI_TASK
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Core;

namespace WeatherSDK.Common.Test
{
    public class ExampleService : IWeatherService
    {
        private readonly Dictionary<string, object> CustomData = new Dictionary<string, object>()
        {
            { "note", "This is the result of the Example service. It returns random values." }
        };
        
        public async UniTask<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken)
        {
            var responseDelay = Random.Range(0.1f, 1f);
            await UniTask.WaitForSeconds(responseDelay, cancellationToken: cancellationToken).SuppressCancellationThrow();
            return new WeatherInfo(
                true, 
                Random.Range(-30.0f, 30.0f),
                $"{nameof(ExampleService)}",
                CustomData);
        }
    }
}
#endif