# Private API (api.core.subscriptions)

This api handles CRUD operations for the subscriptions prove of concept.
It is a pure back-end API and it was disigned to be set up in a private network and behind gateway, so it fully opened and doen't have any kind of authentication flows. Why? - For performance improvement.

### Database

For data storing, the API uses an In-Memory database but If there is a need of persisting the data the API is ready to be poited to an SQL server without changing any line of code (just a small startup configuration).
That's possible because Microsoft Entity Framework was used ad the main ORM for database abstration.

### Email sending

When a subscription request is successfully validated and inserted in the database, the API sends an event message (mail event) into a RabbitMQ cluster so emails can be delivered to the users later on using a more varied kind of services or batches rather than APIs.
A message broker was choice for this solution because events messages are more reliable rather than HTTP requests when it comes failure and "background" data processing.

Considering this approach, when the API starts it tries to connect to a rabbitMQ server and automatically create the exchages/queues  needed for event processing.
You can change the RabbitMQ default queue anytime in the src/appsettings.json configuration file.

# Service Dependencies

This API depends on **rabbitMQ** server for sending event emails 


# How to start it up (StandAlone)

If you have dotnet core SDK 3.1 installed you can just start up the API by running the following command:
```shell
dotnet run --project ./src/api.core.subscriptions.csproj
```

If you don't make sure you have the docker installed and run the following commmands:
```shell
docker build . -t api.core.subscriptions
docker run api.core.subscriptions -p 8000:8080
```
The project is set to run on port 5002 by default
you can change the default port in Properties/launchSettings.json

however if you start the API using docker, the API will run on port 8080.

# Possible troubles during the startup

All exceptions are being handled and the logs are being sent to a SEQ server [https://datalust.co/](https://datalust.co/)
Anyway, the following issues can make you feel confusing if you are not familiar with the project.

## RabbitMQ connection

A rabbit RabbitMQ cluster can take a while to initialize and since the API relies on it, when you perform the first HTTP request the API can take a while for responding

If you get Task Cancellation exception it means that the API is trying to reach the RabbitMQ cluster and the connections are being timedout

Take a look at [./src/appsettings.json](./src/appsettings.json) configuration file in order to check the server hostname/port 

# Swagger Documentation

You can check de API documentation by browing the following address:

[http://localhost:8000/swagger/index.html](http://localhost:8000/swagger/index.html])

*Please consider that the remote port might be different depending on your start up settings.*
