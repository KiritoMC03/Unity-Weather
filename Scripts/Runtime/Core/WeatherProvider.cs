#if UNI_TASK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Location;

namespace WeatherSDK.Core
{
    public class WeatherProvider : IWeatherProvider
    {
        #region Fields

        private readonly IServicesContainer servicesContainer = new HashSetServicesContainer();
        private readonly IWeatherRequestsRunner weatherRequestsRunner = new CollectiveWeatherRequestsRunner();

        #endregion

        #region Constructors

        /// <summary>
        /// Required one not null service
        /// </summary>
        /// <param name="service">New service</param>
        /// <param name="servicesContainer"><see cref="HashSetServicesContainer"/> using by default</param>
        /// <param name="weatherRequestsRunner"><see cref="WeatherSDK.Core.CollectiveWeatherRequestsRunner"/> using by default</param>
        /// <exception cref="System.ArgumentNullException">If <b>service</b> argument is null</exception>
        /// <exception cref="System.ArgumentException">If cannot add <b>service</b> to <b>servicesContainer</b></exception>
        public WeatherProvider(
            IWeatherService service, 
            IServicesContainer servicesContainer = default,
            IWeatherRequestsRunner weatherRequestsRunner = default)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (servicesContainer != null) this.servicesContainer = servicesContainer;
            if (weatherRequestsRunner != null) this.weatherRequestsRunner = weatherRequestsRunner;

            if (!this.servicesContainer.TryAdd(service))
                throw new ArgumentException(CreateAddFailMessage(service));
        }

        /// <summary>
        /// Required minimum one not null service
        /// </summary>
        /// <param name="services">New services collection</param>
        /// <param name="servicesContainer"><see cref="HashSetServicesContainer"/> using by default</param>
        /// <param name="weatherRequestsRunner"><see cref="WeatherSDK.Core.CollectiveWeatherRequestsRunner"/> using by default</param>
        /// <exception cref="System.ArgumentNullException">If <b>services</b> argument is null</exception>
        /// <exception cref="System.ArgumentException">If <b>services</b> collection is empty</exception>
        public WeatherProvider(
            IEnumerable<IWeatherService> services, 
            IServicesContainer servicesContainer = default,
            IWeatherRequestsRunner weatherRequestsRunner = default)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (servicesContainer != null) this.servicesContainer = servicesContainer;
            if (weatherRequestsRunner != null) this.weatherRequestsRunner = weatherRequestsRunner;

            foreach (var service in services)
                if (service != null && !this.servicesContainer.TryAdd(service))
                    Debug.LogWarning(CreateAddFailMessage(service));
            if (!this.servicesContainer.Any())
                throw new ArgumentException($"Collection \"{nameof(services)}\" can`t be empty");
        }
        
        #endregion

        #region IWeatherProvider

        public async UniTask<Weather> GetWeather(
            double latitude, 
            double longitude, 
            CancellationToken cancellationToken = default, 
            float timeout = 7f)
        {
            return await weatherRequestsRunner.StartCollecting(
                servicesContainer,
                new WeatherCoordinates(latitude, longitude),
                timeout,
                cancellationToken);
        }

        public async UniTask<Weather> GetWeather(CancellationToken cancellationToken = default, float timeout = 7)
        {
            var locationLoadingStart = Time.realtimeSinceStartup;
            var (isTimeout, locationInfo) = await new LocationInfoGetter()
                .TryRequestLocation(cancellationToken)
                .TimeoutWithoutException(TimeSpan.FromSeconds(timeout));

            var leftTimeout = timeout - (Time.realtimeSinceStartup - locationLoadingStart);
            if (isTimeout || leftTimeout < 0.0f || !locationInfo.HasValue)
                return Weather.Empty();

            return await GetWeather(locationInfo.Value.latitude, locationInfo.Value.longitude, cancellationToken, leftTimeout);
        }

        /// <summary>
        /// Required not null service
        /// </summary>
        /// <param name="service">New service</param>
        public AddServiceResult AddService(IWeatherService service)
        {
            if (service == null)
                return new AddServiceResult(AddServiceResultState.Failed, AddServiceFailReason.ServiceIsNull);
            if (servicesContainer.TryAdd(service))
                return new AddServiceResult(AddServiceResultState.Success);
            return new AddServiceResult(AddServiceResultState.Failed, AddServiceFailReason.CantAddToContainer);
        }

        #endregion

        #region Methods

        private string CreateAddFailMessage(IWeatherService service)
        {
            var items = new StringBuilder(100);
            foreach (var current in servicesContainer)
            {
                items.Append(current.GetType());
                items.Append(", ");
            }

            return $"Cannot add {nameof(service)}, {service} to {nameof(servicesContainer)}, {servicesContainer} [{items}]";
        }

        #endregion
    }
}
#endif