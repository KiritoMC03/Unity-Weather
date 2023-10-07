using System.Threading;
using System.Threading.Tasks;

namespace WeatherSDK.Core
{
    public interface IWeatherServiceNoDep
    {
        Task<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken);
    }
}