using System;
using System.Threading;
using Cysharp.Threading.Tasks;
namespace WeatherSDK.Core
{
    internal class WeatherRequestInProcessing
    {
        public event Action OnListenersEnded;
        
        private WeatherInfo? weatherInfo;
        private int listeners = 0;
        
        public async void StartProcessing(IWeatherService service, WeatherCoordinates coordinates, CancellationToken token)
        {
            var request = new WeatherRequest(service, coordinates);
            weatherInfo = await new ExponentialBackoffWeatherRequester().StartRequests(request, token);
        }
        
        public async UniTask<WeatherInfo> WaitForInfo(CancellationToken cancellationToken)
        {
            listeners++;
            await UniTask.WaitUntil(WeatherAvailable, cancellationToken: cancellationToken);
            listeners--;
            if (listeners == 0) OnListenersEnded?.Invoke();
            return weatherInfo.Value;
            
            bool WeatherAvailable() => weatherInfo.HasValue;
        }
    }
}