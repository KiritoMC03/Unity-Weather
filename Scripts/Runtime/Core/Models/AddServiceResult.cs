namespace WeatherSDK.Core
{
    public readonly struct AddServiceResult
    {
        public readonly AddServiceResultState state;
        public readonly AddServiceFailReason failReason;

        public AddServiceResult(
            AddServiceResultState state,
            AddServiceFailReason failReason = AddServiceFailReason.None)
        {
            this.state = state;
            this.failReason = failReason;
        }
    }

    public enum AddServiceResultState
    {
        Success,
        Failed,
    }

    public enum AddServiceFailReason
    {
        None,
        ServiceIsNull,
        CantAddToContainer,
    }
}