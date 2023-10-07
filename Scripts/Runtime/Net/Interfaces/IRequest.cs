#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Net
{
    public interface IRequest<TResult>
        where TResult: IRequestResult
    {
        UniTask<TResult> Run(CancellationToken cancellationToken);
    }
}
#endif