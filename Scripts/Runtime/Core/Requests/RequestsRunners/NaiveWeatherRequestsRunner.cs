#if UNI_TASK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Core
{
    /// <summary>
    /// Simple WeatherInfo collector. It creates web request for every service and every StartCollecting() call.
    /// Use can see optimized variant: <see cref="CollectiveWeatherRequestsRunner"/>
    /// </summary>
    public class NaiveWeatherRequestsRunner : IWeatherRequestsRunner
    {
        #region IWeatherRequestsRunner

        public async UniTask<Weather> StartCollecting(
            IServicesContainer services, 
            WeatherCoordinates coordinates, 
            double timeout,
            CancellationToken cancellationToken)
        {
            var localTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var localToken = localTokenSource.Token;
            localTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));

            var capacity = services.Count();
            var tasks = new List<UniTask<WeatherInfo>>(capacity);
            foreach (var service in services)
                tasks.Add(service.GetWeather(coordinates, localToken));

            var (isCanceled, result) = await UniTask.WhenAll(tasks).SuppressCancellationThrow();
            return new Weather(ParseWeatherInfo(result));
        }

        #endregion

        #region Methods

        private List<WeatherInfo> ParseWeatherInfo(IReadOnlyCollection<WeatherInfo> array)
        {
            var result = new List<WeatherInfo>(array.Count);
            foreach (var item in array)
                if (item.IsDataAccepted)
                    result.Add(item);
            return result;
        }

        #endregion
    }
}
#endif