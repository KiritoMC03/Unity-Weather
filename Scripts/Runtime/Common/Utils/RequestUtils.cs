#if UNI_TASK
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace WeatherSDK.Common.Utils
{
    public class RequestUtils
    {
        public static async UniTask<string> SendRequest(string url, CancellationToken cancellationToken)
        {
            var response = await UnityWebRequest
                .Get(url)
                .SendWebRequest()
                .WithCancellation(cancellationToken);
            return response.result switch
            {
                UnityWebRequest.Result.Success => response.downloadHandler.text,
                _ => default
            };
        }
    }
}
#endif