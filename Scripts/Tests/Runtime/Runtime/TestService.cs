#if UNI_TASK
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using WeatherSDK.Core;

namespace WeatherSDK.Tests.Runtime
{
    public class TestService : IWeatherService
    {
        public readonly bool isDataAccepted;
        public readonly float temperature;
        public readonly string source;
        public readonly Dictionary<string, object> customData;

        public TestService(
            bool isDataAccepted, 
            float temperature, 
            string source, 
            Dictionary<string, object> customData = default)
        {
            this.isDataAccepted = isDataAccepted;
            this.temperature = temperature;
            this.source = source;
            this.customData = customData;
        }
        
        public async UniTask<WeatherInfo> GetWeather(WeatherCoordinates coordinates, CancellationToken cancellationToken)
        {
            return new WeatherInfo(isDataAccepted, temperature, source, customData);
        }

        public static TestService Default()
        {
            return new TestService(true, 10.1f, typeof(TestService).ToString());
        }

        public static TestService WithEmptyResult()
        {
            return new TestService(false, 0f, string.Empty);
        }
    }
}
#endif