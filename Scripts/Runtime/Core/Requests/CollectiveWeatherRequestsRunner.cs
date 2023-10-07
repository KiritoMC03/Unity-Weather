#if UNI_TASK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using ExponentialBackoffWeatherRequester = WeatherSDK.Net.ExponentialBackoffRequester<WeatherSDK.Core.WeatherRequest, WeatherSDK.Core.WeatherInfo>;

namespace WeatherSDK.Core
{
    /// <summary>
    /// This class is a complete module for assembling weather data from a list of services. 
    /// This class is optimized to work with frequent weather requests. 
    /// For example, this call will not create 100 requests for each service,
    /// but will create 1 request, and the response to this request will
    /// return all 100 requests made during the request-response time.
    /// </summary> 
    /// <code>
    ///     loop (100 times) {
    ///         CollectiveWeatherRequestsRunner.StartCollecting()
    ///     }
    /// </code>
    public class CollectiveWeatherRequestsRunner : IWeatherRequestsRunner
    {
        private Dictionary<IWeatherService, Dictionary<WeatherCoordinates, WeatherRequestInProcessing>> processingRequests = 
            new Dictionary<IWeatherService, Dictionary<WeatherCoordinates, WeatherRequestInProcessing>>();

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
            var tasks = new List<UniTask>(capacity);
            var results = new List<WeatherInfo>(capacity);
            foreach (var service in services)
            {
                if (TryGetExistRequestTask(service, coordinates, localToken, out var task)) { }
                else CreateNewRequest(service, coordinates, localToken, out task);
                tasks.Add(task.ContinueWith(AddToResults));
                void AddToResults(WeatherInfo info) => results.Add(info);
            }

            var isCanceled = await UniTask.WhenAll(tasks).SuppressCancellationThrow();
            ParseWeatherInfo(results);
            return new Weather(results);
        }

        private void ParseWeatherInfo(List<WeatherInfo> infoList)
        {
            infoList.RemoveAll(item => !item.IsDataAccepted);
        }

        private void CreateNewRequest(
            IWeatherService service, 
            WeatherCoordinates coordinates, 
            CancellationToken token, 
            out UniTask<WeatherInfo> requestTask)
        {
            var newRequest = new WeatherRequestInProcessing();
            requestTask = newRequest.WaitForInfo(token);
            newRequest.StartProcessing<ExponentialBackoffWeatherRequester>(service, coordinates, token);
            if (processingRequests.TryGetValue(service, out var reqByCoords)) { }
            else
            {
                reqByCoords = new Dictionary<WeatherCoordinates, WeatherRequestInProcessing>();
                processingRequests.Add(service, reqByCoords);
            }
            reqByCoords[coordinates] = newRequest;
            newRequest.OnListenersEnded += Remove;

            void Remove() => processingRequests[service].Remove(coordinates);
        }

        private bool TryGetExistRequestTask(
            IWeatherService service, 
            WeatherCoordinates coordinates, 
            CancellationToken token, 
            out UniTask<WeatherInfo> existTask)
        {
            if (processingRequests.TryGetValue(service, out var reqByCoords) &&
                reqByCoords.TryGetValue(coordinates, out var weatherRequestInProcessing))
            {
                existTask = weatherRequestInProcessing.WaitForInfo(token);
                return true;
            }

            existTask = default;
            return false;
        }
    }
}
#endif