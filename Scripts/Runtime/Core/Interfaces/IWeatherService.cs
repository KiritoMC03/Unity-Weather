using System.Threading;
using System.Threading.Tasks;

namespace WeatherSDK.Core
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken);
    }
}