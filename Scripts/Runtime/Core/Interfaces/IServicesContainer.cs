using System.Collections.Generic;

namespace WeatherSDK.Core
{
    public interface IServicesContainer : IEnumerable<IWeatherService>
    {
        bool TryAdd(IWeatherService service);
    }
}