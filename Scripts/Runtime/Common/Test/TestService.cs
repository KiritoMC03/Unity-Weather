using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Core;
namespace Common.Test
{
    public class TestService : IWeatherService
    {
        private static int i;
        
        public async Task<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken)
        {
            Random.InitState(++i);
            var delay = Random.Range(1, 2);
            // Debug.Log(delay);
            await UniTask.WaitForSeconds(0.5f);
            return new WeatherInfo() { isInitialized = false };
        }
    }
}