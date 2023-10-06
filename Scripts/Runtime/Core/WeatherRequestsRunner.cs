using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Core
{
    internal class WeatherRequestsRunner
    {
        private Dictionary<IWeatherService, Dictionary<WeatherCoordinates, WeatherRequestInProcessing>>
            processingRequests = new();

        public async UniTask<Weather> StartCollecting(IServicesContainer services, 
            WeatherCoordinates coordinates, 
            double timeout,
            CancellationToken cancellationToken)
        {
            var localTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var localToken = localTokenSource.Token;
            localTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));

            var tasks = new List<UniTask<WeatherInfo>>();
            foreach (var service in services)
            {
                if (TryGetExistRequestTask(service, coordinates, localToken, out var task)) { }
                else CreateNewRequest(service, coordinates, localToken, out task);
                tasks.Add(task);
            }

            var results = new List<WeatherInfo>(await UniTask.WhenAll(tasks));
            ParseWeatherInfo(results);
            return new Weather(results);
        }

        private void ParseWeatherInfo(List<WeatherInfo> infoList)
        {
            infoList.RemoveAll(item => !item.isInitialized);
        }

        private void CreateNewRequest(
            IWeatherService service, 
            WeatherCoordinates coordinates, 
            CancellationToken token, 
            out UniTask<WeatherInfo> requestTask)
        {
            var newRequest = new WeatherRequestInProcessing();
            requestTask = newRequest.WaitForInfo(token);
            newRequest.StartProcessing(service, coordinates, token);
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