#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WeatherSDK.Net
{
    public struct ExponentialBackoffRequester<T, TResult> : IRequester<T, TResult>
        where T: IRequest<TResult>
        where TResult: IRequestResult
    {
        private const float BasePower = 2f;
        private const float MaxRetryDelay = 60f * 60f * 24f; // One day =)

        public async UniTask<TResult> StartRequests(T request, CancellationToken cancellationToken)
        {
            var retryAttempt = 0;
            TResult result;
            do
            {
                result = await request.Run(cancellationToken);
                var delay = Mathf.Pow(BasePower, retryAttempt);
                if (delay >= MaxRetryDelay)
                {
                    Debug.LogWarning($"Max retry delay ({MaxRetryDelay} sec) was reached! Requests stopped.");
                    return result;
                }
                var isCancelled = await UniTask
                    .WaitForSeconds(delay, true, cancellationToken: cancellationToken)
                    .SuppressCancellationThrow();
                if (isCancelled) 
                    return default;
                retryAttempt++;
            }
            while (!result.IsDataAccepted);
            return result;
        }
    }
}
#endif