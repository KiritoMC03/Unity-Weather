#if UNI_TASK
using System.Collections.Generic;

namespace WeatherSDK.Core
{
    public class HashSetServicesContainer : HashSet<IWeatherService>, IServicesContainer
    {
        bool IServicesContainer.TryAdd(IWeatherService service)
        {
            return this.Add(service);
        }
    }
}
#endif