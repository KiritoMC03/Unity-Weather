namespace WeatherSDK.Core
{
    public readonly struct AddServiceResult
    {
        public readonly AddServiceResultState state;
    }

    public enum AddServiceResultState
    {
        Success,
        Failed,
    }
}