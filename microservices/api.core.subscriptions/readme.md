# Private API (api.core.subscriptions)

This API performs CRUD operations for the subscription's proof of concept.

It is a pure back-end API and it was designed to be set in a private network and behind a secured gateway. It is fully opened and doesn't support authentication validations for performance improving. 
It's assumed that the API is secured by gateways and private networking.

### Database

For data storing, the API uses an In-Memory database (for demo purposes). 

If there is a need of persisting data the API is ready to be pointed to an SQL server without changing any line of code (just a small startup configuration).
That's possible because Microsoft Entity Framework was used as the main ORM for database abstraction.

Since it was used generic DB providers and the Dependency injection pattern, the changes are resumed to small changes in the API bootstrap.



**in example:**

> An In-Memory database implementation like this:

```csharp
    services.AddDbContext<SubscriptionsDb>(
        options => options.UseInMemoryDatabase());
```
>can be changed to the following implementation, in order to use a database server:  

```csharp
    services.AddDbContext<CatalogContext>(
        options => options.UseSqlServer (
            Configuration.GetConnectionString("DefaultConnection")));
```

https://bitbucket.org/jsoliveira/iban-services-poc/src/8030495d8d3cb28e2d83875e272b14caf67c2940/microservices/api.core.subscriptions/src/Startup.cs#lines-70


<br/>

## Email sending
---
When a subscription creation/deleting request performed, validated and inserted in the database, the API sends an event message (mail event) into the RabbitMQ cluster.

A message broker was chosen for this solution because event messages are more reliable rather than HTTP requests when it comes failure and "background" data processing.

By using message queues, emails can be delivered to the users (in a distributed way) by other services or batches that might be more focused on mailing processing. This way is more reliable sharing the workload across a more varied kind of services and if, for some reason, the "leaf" services go down, we still have the possibility of keep tracking the events (mails) and resuming their processing when the services go up again. 

<br/>

# Service Dependencies

This API relies only on **rabbitMQ** server for event sending.

# How to start it up (StandAlone)

If you have dotnet core SDK 3.1 installed you can just start the API by running the following command:
```shell
dotnet run --project src/api.core.subscriptions.csproj
```

If you want to use docker instead, make sure you have it installed and run the following commands
```shell
docker build . -t api.core.subscriptions
docker run api.core.subscriptions -p 8000:8080
```

The project is set to run on port 5002 by default and it be changed [Properties/launchSettings.json](Properties/launchSettings.json) 


_When running the API inside a container the default port is set to 8080_

# Swagger Documentation

Once you start the API you can check de API documentation by browsing the following address:

[http://localhost:5002/swagger/](http://localhost:5002/swagger/)

Please consider that the remote port might be different depending on your startup settings. 

>The port 8080 is used when the API runs inside docker containers.

>Check the ASPNETCORE_URL environment variable inside the dockerfile and the docker port mappings before you go (docker run api.core.subscriptions -p 8000:8080)

[http://localhost:8000/swagger/](http://localhost:8000/swagger/)


# Possible troubles during the startup

All exceptions are handled and the logs are sent to SEQ logging server [https://datalust.co/](https://datalust.co/)

The following issues can help you figure out troubles with this API if you are not familiar with the project.

## RabbitMQ connection

A rabbit RabbitMQ cluster can take a while to initialize (up to 2min). 
Since the API relies on it, if the cluster is down or still initializing, when you perform an HTTP request to the api, it might fail due to timeouts (30secs) or can take a while to give the response.

If you get a Task Cancellation exception it means that the API is trying to reach the RabbitMQ cluster and the connections are being refused or timed out

So what to do:

>Take a look at [src/appsettings.json](src/appsettings.json) configuration file and check the cluster hostname and remote port.

>Check if the RabbitMQ is up and running

>Check if the configured port are exposed in the containers

