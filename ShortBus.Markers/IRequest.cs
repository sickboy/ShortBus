namespace ShortBus
{
    public interface IRequest<TResponseData> {}

    public interface IRequest : IRequest<UnitType> { }
}