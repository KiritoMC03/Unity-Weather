using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Utils
{
    // public struct GenericTimeLimitedTaskRunner
    // {
    //     private CancellationTokenSource tokenSource;
    //     
    //     public async UniTask<TResult> Run<TResult>(
    //         Func<CancellationToken, UniTask<TResult>> func, 
    //         CancellationToken cancellationToken, 
    //         double time)
    //     {
    //         if (cancellationToken.IsCancellationRequested)
    //             throw new ArgumentException("CancellationToken already cancelled");
    //         if (func == null)
    //             throw new ArgumentNullException(nameof(func));
    //
    //         tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(time));
    //         cancellationToken.Register(OnMainTokenCancelled);
    //         return await func.Invoke(tokenSource.Token);
    //     }
    //
    //     private void OnMainTokenCancelled() => tokenSource.Cancel();
    // }
}