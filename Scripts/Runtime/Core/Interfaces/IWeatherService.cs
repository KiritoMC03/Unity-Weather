#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Core
{
    public interface IWeatherService
    {
        UniTask<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken);
    }
}
#endif