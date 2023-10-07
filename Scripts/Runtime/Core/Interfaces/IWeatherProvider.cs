#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Core
{
    public interface IWeatherProvider
    {
        UniTask<Weather> GetWeather(double latitude, double longitude, CancellationToken cancellationToken = default, float timeout = 7f);
        UniTask<Weather> GetWeather(CancellationToken cancellationToken = default, float timeout = 7f);
        AddServiceResult AddService(IWeatherService service);
    }
}
#endif