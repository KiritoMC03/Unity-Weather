#if UNI_TASK
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WeatherSDK.Net;

namespace WeatherSDK.Core
{
    /// <summary>
    /// This class only runs a specific weather service with CancellationToken 
    /// </summary>
    public readonly struct WeatherRequest : IRequest<WeatherInfo>
    {
        private readonly IWeatherService service;
        private readonly WeatherCoordinates coordinates;

        public WeatherRequest(IWeatherService service, WeatherCoordinates coordinates)
        {
            this.service = service;
            this.coordinates = coordinates;
        }

        /// <remarks>
        /// Contains try-catch only in UnityEditor mode or in Development Build
        /// (UNITY_EDITOR || DEVELOPMENT_BUILD preprocessing directives)
        /// </remarks>
        public async UniTask<WeatherInfo> Run(CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            try
            {
                return await service.GetWeather(coordinates, cancellationToken);
            }
            catch (Exception e)
            {
                Debug.LogError($"Caught exception in the {service.GetType()} service: \n{e}");
                return WeatherInfo.Empty();
            }
            #endif
            return await service.GetWeather(coordinates, cancellationToken);
        }
    }
}
#endif