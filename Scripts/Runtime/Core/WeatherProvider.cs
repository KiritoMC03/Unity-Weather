using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherSDK.Core
{
    public class WeatherProvider : IWeatherProvider
    {
        #region Fields

        private readonly IServicesContainer servicesContainer = new ServicesContainer();
        private readonly WeatherRequestsRunner weatherRequestsRunner = new WeatherRequestsRunner();

        #endregion

        #region Constructors

        public WeatherProvider(IWeatherService service)
        {
            servicesContainer.TryAdd(service);
        }

        public WeatherProvider(IEnumerable<IWeatherService> services)
        {
            foreach (var service in services)
                servicesContainer.TryAdd(service);
            if (!servicesContainer.Any())
                throw new ArgumentException($"Collection \"{nameof(services)}\" can`t be empty");
        }

        #endregion

        #region IWeatherProvider

        public async Task<Weather> GetWeather(double latitude, double longitude, float timeout, CancellationToken cancellationToken)
        {
            return await weatherRequestsRunner.StartCollecting(
                servicesContainer,
                new WeatherCoordinates(latitude, longitude),
                timeout,
                cancellationToken);
        }

        public AddServiceResult AddService(IWeatherService service)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}