## ShortBus
ShortBus is an in-process mediator with low-friction API from mhinze https://github.com/mhinze

### Notification
    public class DoSomething : INotification { }

	public class DoesSomething : INotificationHandler<DoSomething> {
		public void Handle(DoSomething command) {
		   // does something
		}
	}

    _mediator.Notify(new DoSomething());

### Async Notification
    public class DoSomething : IAsyncNotification { }

	public class DoesSomething : IAsyncNotificationHandler<DoSomething> {
		public async Task Handle(DoSomething command) {
		   // await ... does something
		}
	}

    await _mediator.NotifyAsync(new DoSomething());

### Request/Command
    public class AskAQuestion : IRequest<Answer> { }

	public class Answerer : IRequestHandler<AskAQuestion, Answer> {
	    public Answer Handle(AskAQuestion request) {			
			return answer;
		}
	}

	var answer = _mediator.Request(new AskAQuestion());
	

### Async Request/Command
    public class AskAQuestion : IAsyncRequest<Answer> { }

	public class Answerer : IAsyncRequestHandler<AskAQuestion, Answer> {
	    public async Task<Answer> Handle(AskAQuestion request) {			
			return await ... answer;
		}
	}

	var answer = await _mediator.RequestAsync(new AskAQuestion());

### Catch Exceptions and package into ResponseObject
	ShortBus.Response response = await _mediator.RequestWithResponseAsync(new AskAQuestion());
    var answer = response.Data;
    var exception = response.Exception;
    var hasException = response.HasException();

### StructureMap configuration
ShortBus can use different IoC containers. StructureMap for example requires that you register 
handlers:

    ObjectFactory.Initialize(i => i.Scan(s =>
    {
        s.AssemblyContainingType<IMediator>();
        s.TheCallingAssembly();
        s.WithDefaultConventions();
        s.ConnectImplementationsToTypesClosing( ( typeof ( IRequestHandler<,> ) ) );
        s.ConnectImplementationsToTypesClosing( ( typeof ( IAsyncRequestHandler<,> ) ) );
        s.AddAllTypesOf( typeof ( INotificationHandler<> ) );
        s.AddAllTypesOf( typeof ( IAsyncNotificationHandler<> ) );
    }));	

Configuration examples for other IoC containers can be found in the test project.

### Low-friction API
No type parameter noise.

### What for?

* Query objects
* Enables subcutaneous testing
* Business concepts as first class citizens

### In Production
ShortBus is in production powering the server APIs for major ecommerce applications.
