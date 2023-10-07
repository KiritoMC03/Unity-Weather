using System.Collections.Generic;

namespace WeatherSDK.Core
{
    public struct Weather
    {
        public readonly List<WeatherInfo> infoList;

        public Weather(List<WeatherInfo> infoList)
        {
            this.infoList = infoList;
        }
    }
}