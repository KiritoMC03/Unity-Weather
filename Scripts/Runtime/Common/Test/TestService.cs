#if UNI_TASK
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Core;

namespace Common.Test
{
    public class TestService : IWeatherService
    {
        private static int i = 53155;
        
        public async Task<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken)
        {
            Random.InitState(++i);
            var delay = Random.Range(1, 13);
            await UniTask.WaitForSeconds(delay);
            return new WeatherInfo(true);
        }
    }
}
#endif