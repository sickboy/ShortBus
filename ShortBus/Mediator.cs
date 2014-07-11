using Microsoft.CSharp.RuntimeBinder;

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
            if (dependencyResolver == null)
                throw new ArgumentNullException("dependencyResolver");

            _dependencyResolver = dependencyResolver;
            HandlerInstanceBuilder = dependencyResolver.GetInstance;
        }

        public virtual TResponseData Request<TResponseData>(IRequest<TResponseData> request) {
            if (request == null)
                throw new ArgumentNullException("request");

            try {
                return
                    HandlerInstanceBuilder(typeof (IRequestHandler<,>).MakeGenericType(request.GetType(),
                        typeof (TResponseData))).Handle((dynamic) request);
            } catch (RuntimeBinderException ex) {
                if (ex.Message.Contains("'Handle'"))
                    throw WrapRuntimeBinderException(ex);
                throw;
            }
        }

        public Task<TResponseData> RequestAsync<TResponseData>(IAsyncRequest<TResponseData> request) {
            if (request == null)
                throw new ArgumentNullException("request");

            try {
                return
                    HandlerInstanceBuilder(typeof (IAsyncRequestHandler<,>).MakeGenericType(request.GetType(),
                        typeof (TResponseData))).HandleAsync((dynamic) request);
            } catch (RuntimeBinderException ex) {
                if (ex.Message.Contains("'HandleAsync'"))
                    throw WrapRuntimeBinderException(ex);
                throw;
            }
        }

        public void Notify<TNotification>(TNotification notification) {
            if (notification == null)
                throw new ArgumentNullException("notification");

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
            if (notification == null)
                throw new ArgumentNullException("notification");

            var handlers = _dependencyResolver.GetInstances<IAsyncNotificationHandler<TNotification>>();

            return Task.WhenAll(handlers.Select(x => x.HandleAsync(notification)));
        }

        static MediatorMethodResolveException WrapRuntimeBinderException(RuntimeBinderException ex) {
            return new MediatorMethodResolveException(
                "RuntimeBinderException occurred, this usually occurs if the request handler is Internal, either make it public or add the InternalsVisibleTo assembly attribute, with 'ShortBus' as target", ex);
        }

        class MediatorMethodResolveException : Exception
        {
            public MediatorMethodResolveException(string message, Exception inner) : base(message, inner) { }
        }
    }
}