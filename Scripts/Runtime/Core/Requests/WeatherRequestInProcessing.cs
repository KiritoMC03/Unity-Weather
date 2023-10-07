#if UNI_TASK
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using WeatherSDK.Net;

namespace WeatherSDK.Core
{
    internal class WeatherRequestInProcessing
    {
        public event Action OnListenersEnded;
        
        private WeatherInfo? weatherInfo;
        private int listeners = 0;

        public async void StartProcessing<TRequester>(
            IWeatherService service, 
            WeatherCoordinates coordinates, 
            CancellationToken token)
        where TRequester: IRequester<WeatherRequest, WeatherInfo>, new()
        {
            var request = new WeatherRequest(service, coordinates);
            weatherInfo = await new TRequester().StartRequests(request, token);
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
#endif