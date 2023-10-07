using WeatherSDK.Net;

namespace WeatherSDK.Core
{
    public struct WeatherInfo : IRequestResult
    {
        public bool IsDataAccepted { get; }

        public WeatherInfo(bool isDataAccepted)
        {
            IsDataAccepted = isDataAccepted;
        }
    }
}