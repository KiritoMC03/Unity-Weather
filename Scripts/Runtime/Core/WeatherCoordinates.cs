namespace WeatherSDK.Core
{
    public struct WeatherCoordinates
    {
        public readonly double latitude;
        public readonly double longitude;

        public WeatherCoordinates(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}