using System.Collections;
using System.Collections.Generic;

namespace WeatherSDK.Core
{
    public struct Weather : IEnumerable<WeatherInfo>
    {
        public readonly List<WeatherInfo> infoList;

        public Weather(List<WeatherInfo> infoList)
        {
            this.infoList = infoList;
        }

        public IEnumerator<WeatherInfo> GetEnumerator() => infoList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}