namespace ShortBus
{
    public interface IAsyncRequest<TResponseData> {}

    public interface IAsyncRequest : IAsyncRequest<UnitType> {}
}