#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Net
{
    public interface IRequester<in T, TResult>
        where T: IRequest<TResult>
        where TResult: IRequestResult
    {
        UniTask<TResult> StartRequests(T request, CancellationToken cancellationToken);
    }
}
#endif