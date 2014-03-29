namespace ShortBus
{
    using System.Threading.Tasks;

    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }

    public interface IAsyncRequestHandler<in TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }

    public interface INotificationHandler<in TNotification>
    {
        void Handle(TNotification notification);
    }

    public interface IAsyncNotificationHandler<in TNotification>
    {
        Task HandleAsync(TNotification notification);
    }

    public abstract class AsyncCommandHandler<TMessage> : IAsyncRequestHandler<TMessage, UnitType>
        where TMessage : IAsyncRequest<UnitType>
    {
        public async Task<UnitType> HandleAsync(TMessage message) {
            await HandleAsyncCore(message).ConfigureAwait(false);

            return UnitType.Default;
        }

        protected abstract Task HandleAsyncCore(TMessage message);
    }

    public abstract class CommandHandler<TMessage> : IRequestHandler<TMessage, UnitType>
        where TMessage : IRequest<UnitType>
    {
        public UnitType Handle(TMessage message) {
            HandleCore(message);

            return UnitType.Default;
        }

        protected abstract void HandleCore(TMessage message);
    }
}