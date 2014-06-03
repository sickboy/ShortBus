namespace ShortBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMediator
    {
        TResponseData Request<TResponseData>(IRequest<TResponseData> request);
        Task<TResponseData> RequestAsync<TResponseData>(IAsyncRequest<TResponseData> request);

        void Notify<TNotification>(TNotification notification);
        Task NotifyAsync<TNotification>(TNotification notification);
    }

    public class Mediator : IMediator
    {
        readonly Func<Type, dynamic> HandlerInstanceBuilder;
        readonly IDependencyResolver _dependencyResolver;

        public Mediator(IDependencyResolver dependencyResolver) {
            _dependencyResolver = dependencyResolver;
            HandlerInstanceBuilder = dependencyResolver.GetInstance;
        }

        public virtual TResponseData Request<TResponseData>(IRequest<TResponseData> request) {
            return
                HandlerInstanceBuilder(typeof (IRequestHandler<,>).MakeGenericType(request.GetType(),
                    typeof (TResponseData))).Handle((dynamic) request);
        }

        public Task<TResponseData> RequestAsync<TResponseData>(IAsyncRequest<TResponseData> request) {
            return
                HandlerInstanceBuilder(typeof (IAsyncRequestHandler<,>).MakeGenericType(request.GetType(),
                    typeof (TResponseData))).HandleAsync((dynamic) request);
        }

        public void Notify<TNotification>(TNotification notification) {
            var handlers = _dependencyResolver.GetInstances<INotificationHandler<TNotification>>();

            List<Exception> exceptions = null;

            foreach (var handler in handlers)
                try {
                    handler.Handle(notification);
                } catch (Exception e) {
                    (exceptions ?? (exceptions = new List<Exception>())).Add(e);
                }

            if (exceptions != null)
                throw new AggregateException(exceptions);
        }

        public Task NotifyAsync<TNotification>(TNotification notification) {
            var handlers = _dependencyResolver.GetInstances<IAsyncNotificationHandler<TNotification>>();

            return Task.WhenAll(handlers.Select(x => x.HandleAsync(notification)));
        }
    }
}