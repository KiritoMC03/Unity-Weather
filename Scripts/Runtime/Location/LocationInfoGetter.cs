#if UNI_TASK
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace WeatherSDK.Location
{
    public struct LocationInfoGetter
    {
        public async UniTask<LocationInfo?> TryRequestLocation(CancellationToken cancellationToken)
        {
            if (HandlePermission())
                return await RequestLocationInternal(cancellationToken);

            return default;
        }

        private async UniTask<LocationInfo> RequestLocationInternal(CancellationToken cancellationToken)
        {
            Input.location.Start();
            await WaitInitialization(cancellationToken);
            if (cancellationToken.IsCancellationRequested ||
                Input.location.status != LocationServiceStatus.Running)
            {
                return default;
            }

            return Input.location.lastData;
        }

        private async UniTask WaitInitialization(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested &&
                Input.location.status is LocationServiceStatus.Initializing)
            {
                await UniTask.Yield();
            }
        }

        private static bool HandlePermission()
        {
            if (Input.location.isEnabledByUser)
                return true;

            #if UNITY_ANDROID

            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation)) {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
            }

            if (!UnityEngine.Input.location.isEnabledByUser) {
                Debug.LogWarning("Location not enabled by user");
                return false;
            }

            #endif

            return false;
        }
    }
}

#endif