using System.Collections.Generic;

namespace WeatherSDK.Core
{
    internal class ServicesContainer : HashSet<IWeatherService>, IServicesContainer
    {
        bool IServicesContainer.TryAdd(IWeatherService service)
        {
            return this.Add(service);
        }
    }
}