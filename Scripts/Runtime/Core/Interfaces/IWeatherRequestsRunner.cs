#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Core
{
    public interface IWeatherRequestsRunner
    {
        UniTask<Weather> StartCollecting(
            IServicesContainer services,
            WeatherCoordinates coordinates,
            double timeout,
            CancellationToken cancellationToken);
    }
}
#endif