using System.Collections.Generic;
using System.Text;
using WeatherSDK.Net;

namespace WeatherSDK.Core
{
    public struct WeatherInfo : IRequestResult
    {
        private const char NewLine = '\n';
        private const char Tab = '\t';
        private const char OpenBracket = '{';
        private const char CloseBracket = '}';
        private const string SourceKey = "source: ";
        private const string TemperatureKey = "temperature: ";
        private const string KeyValueSeparator = ": ";
        
        public bool IsDataAccepted { get; }

        public readonly float temperature;
        public readonly string source;
        public readonly Dictionary<string, object> customData;

        public WeatherInfo(
            bool isDataAccepted, 
            float temperature, 
            string source, 
            Dictionary<string, object> customData = default)
        {
            IsDataAccepted = isDataAccepted;
            this.temperature = temperature;
            this.source = source;
            this.customData = customData;
        }

        public static WeatherInfo Empty() => new WeatherInfo();

        public override string ToString()
        {
            if (IsDataAccepted)
                return BuildString();
            return $"{typeof(WeatherInfo)} {{ Empty }}";
        }

        private string BuildString()
        {
            var builder = new StringBuilder(100);
            builder
                .Append(typeof(WeatherInfo))
                .Append(NewLine)
                .Append(OpenBracket);
            builder
                .Append(NewLine)
                .Append(Tab)
                .Append(SourceKey)
                .Append(source)

                .Append(NewLine)
                .Append(Tab)
                .Append(TemperatureKey)
                .Append(temperature);
            if (customData != null)
            {
                foreach (var data in customData)
                {
                    builder
                        .Append(NewLine)
                        .Append(Tab)
                        .Append(data.Key)
                        .Append(KeyValueSeparator)
                        .Append(data.Value);
                }
            }
            builder
                .Append(NewLine)
                .Append(CloseBracket);
            return builder.ToString();
        }
    }
}