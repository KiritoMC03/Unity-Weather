using System.Threading;
using System.Threading.Tasks;

namespace WeatherSDK.Core
{
    public interface IWeatherProvider
    {
        Task<Weather> GetWeather(double latitude, double longitude, float timeout, CancellationToken cancellationToken);
        AddServiceResult AddService(IWeatherService service);
    }
}