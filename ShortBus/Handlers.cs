namespace ShortBus
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    [ContractClass(typeof (RequestHandlerContract<,>))]
    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }

    [ContractClassFor(typeof (IRequestHandler<,>))]
    public abstract class RequestHandlerContract<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public TResponse Handle(TRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            return default(TResponse);
        }
    }

    [ContractClass(typeof (AsyncRequestHandlerContract<,>))]
    public interface IAsyncRequestHandler<in TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }

    [ContractClassFor(typeof (IAsyncRequestHandler<,>))]
    public abstract class AsyncRequestHandlerContract<TRequest, TResponse>
        : IAsyncRequestHandler<TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {
        public Task<TResponse> HandleAsync(TRequest request)
        {
            Contract.Requires<ArgumentNullException>(request != null);
            return default(Task<TResponse>);
        }
    }


    [ContractClass(typeof (NotificationHandlerContract<>))]
    public interface INotificationHandler<in TNotification>
    {
        void Handle(TNotification notification);
    }

    [ContractClassFor(typeof (INotificationHandler<>))]
    public abstract class NotificationHandlerContract<TNotification>
        : INotificationHandler<TNotification>
    {
        public void Handle(TNotification notification)
        {
            Contract.Requires<ArgumentNullException>(notification != null);
        }
    }

    [ContractClass(typeof (AsyncNotificationHandlerContract<>))]
    public interface IAsyncNotificationHandler<in TNotification>
    {
        Task HandleAsync(TNotification notification);
    }

    [ContractClassFor(typeof (IAsyncNotificationHandler<>))]
    public abstract class AsyncNotificationHandlerContract<TNotification>
        : IAsyncNotificationHandler<TNotification>
    {
        public Task HandleAsync(TNotification notification)
        {
            Contract.Requires<ArgumentNullException>(notification != null);
            return default(Task);
        }
    }
}