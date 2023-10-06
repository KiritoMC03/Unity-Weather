using System.Collections.Generic;

namespace WeatherSDK.Core
{
    public struct Weather
    {
        private readonly List<WeatherInfo> infoList;

        public Weather(List<WeatherInfo> infoList)
        {
            this.infoList = infoList;

        }

    }

    public struct WeatherInfo
    {
        public bool isInitialized;
    }
}