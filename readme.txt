The solution is composed by 3 main components:

- Tasks.DomainModel: the domain model that is in use in all the other projects. In this project you can find a class diagram that can be helpful to understand how the domain model is structured;

- Tasks.DataAccess: the data access part, implemented using the repository patter, with a simple cache handling implementation;

- Tasks.Infrastructure: the business logic part of the application. I added some simple business logic rules. One of these rules are that when a task is updated/deleted or when a comment or some working session is logged on the task, a notification is sent to every user that is whatching the task.

- Tasks.WS: the WebAPi proejct that implements a REST API to handle the tasks.

Some libraries that I use in these projects:
- Automapper, to map the WebAPI models into the domain model entities;
- Fluent Assertions to the assertinos in the unit tests;
- Moq to mock objects in the unit tests;
- Unity as IoC container;
- Log4Net as log library
- Enterprise Library Validation Application Block to validate the entities using attributes;

***** DOMAIN MODEL
In the Tasks.DomainModel, yoy can find all the classe that define the domain. I defined the domain to be able to:
- handle the single tasks;
- be able to handle also subtasks for a task;
- be able to add comments to the tasks;
- be able to log hours to the tasks;
Every time that a user update a task, delete a task, write a new/edit an existing comment for a task, log hours on a task, the system will send a notification to the watcher users of the task by email.
In the Tasks.DomainModel there is the DomainModel.cd class diagram that can be useful to understand the model.

***** VALIDATION
I created a custom class to validate the entities: Tasks.Infrastructure.Validators.DomainEntityValidator. This class uses the Microsoft Enterprise Library Validation Block to be able to use the validation attributes for the entities.

***** UNIT TEST
Every single project has it's own unit test project.
For Task.WS unit test, we used two different type of unit test:
1. a pure unit test that uses the new IHttpActionResult interface;
2. a unit test that use a in-memory hosting of the web API, for example: TaskControllerTest.Add_ValidTask_ShouldReturnCreatedAndTheJSonWithTheNewTask(). In this case, we are testing a full ASP.NET WebAPI stack. This type of test permits to be able to check the results of the web API call in details.

***** CACHING
I implemented a very basic caching system that uses a Dictionary as in-memory storage. The code can be easly changed to use other type of caching system (Redis). See the Tasks.DataAccess.ICacheProvider and Tasks.DataAccess.CacheProvider.
The implemented way to create the keys for the items in the cache should be refactored in a better way.

***** LOGGING
I implemented a simple loggin system to log the unhandled exception. For this logging system I used the Log4Net library and I configure it to write the log messages in a text file. (See Tasks.WS.Log4NetExceptionLogger)

***** TO BE DEVELOPER/INTEGRATED
- add other controller to manage the comment to the tasks and to log the work session ( I started developing these but I had to rollback because no enough time left)
- security using signed token: we can use a WebAPI HttpMessageHandler implementation to be able to check if a Http request contains a valid signed token and to extract all the information from the token (the user that is sending the request, for example);
- use of the async pattern: to increase scalability we can use the .NET async/await features in this code.
