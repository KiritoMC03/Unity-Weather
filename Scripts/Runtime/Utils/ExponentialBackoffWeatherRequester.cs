using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WeatherSDK.Core
{
    // ToDo: Can try to do this with a generic method in future
    internal struct ExponentialBackoffWeatherRequester
    {
        private const float BaseRetryDelay = 2f;
        private const float MaxRetryDelay = 60f * 60f * 24f; // One day =)
        
        public async UniTask<WeatherInfo> StartRequests(WeatherRequest weatherRequest, CancellationToken cancellationToken)
        {
            var retryAttempt = 0;
            WeatherInfo result;
            do
            {
                result = await weatherRequest.Run(cancellationToken);
                var delay = Mathf.Pow(BaseRetryDelay, retryAttempt);
                if (delay >= MaxRetryDelay)
                {
                    Debug.LogWarning($"Max retry delay ({MaxRetryDelay} sec) was reached! Requests stopped.");
                    result.isInitialized = false;
                    return result;
                }
                var isCancelled = await UniTask
                    .WaitForSeconds(delay, true, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
                if (isCancelled) 
                    return default;
                Debug.Log($"delay: {delay}");
                retryAttempt++;
            }
            while (!result.isInitialized);
            return result;
        }
    }
}