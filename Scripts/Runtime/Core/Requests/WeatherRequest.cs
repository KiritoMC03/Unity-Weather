#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Core
{
    /// <summary>
    /// This class only runs a specific weather service with CancellationToken 
    /// </summary>
    internal readonly struct WeatherRequest
    {
        private readonly IWeatherService service;
        private readonly WeatherCoordinates coordinates;

        public WeatherRequest(IWeatherService service, WeatherCoordinates coordinates)
        {
            this.service = service;
            this.coordinates = coordinates;
        }

        public async UniTask<WeatherInfo> Run(CancellationToken cancellationToken)
        {
            return await service.GetWeather(coordinates, cancellationToken);
        }
    }
}
#endif