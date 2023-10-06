using System.Collections.Generic;

namespace WeatherSDK.Core
{
    internal interface IServicesContainer : IEnumerable<IWeatherService>
    {
        bool TryAdd(IWeatherService service);
    }
}