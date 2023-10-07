using System.Collections;
using System.Collections.Generic;

namespace WeatherSDK.Core
{
    public struct Weather : IEnumerable<WeatherInfo>
    {
        #region Fields

        public readonly List<WeatherInfo> infoList;

        #endregion

        #region Constructors

        public Weather(List<WeatherInfo> infoList)
        {
            this.infoList = infoList;
        }

        #endregion

        #region IEnumerable<WeatherInfo>

        public IEnumerator<WeatherInfo> GetEnumerator() => infoList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Methods

        public static Weather Empty()
        {
            return new Weather(new List<WeatherInfo>(0));
        }

        #endregion
    }
}