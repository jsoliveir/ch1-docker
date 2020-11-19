# Private API (api.core.mail)

This API is basically a consumer of a RabbitMQ message broken.
It looks for new mail event messages sent by services in a specific queue and when a message arrives, it creates and forwards a mail according to the message received

The API was designed to be set up in a private network and behind gateway, so it fully opened and doesn't have any kind of authentication flows

Why? - for performance reasons

## Email sending

The API connects to RabbitMQ cluster and waits for an event message came from a specific queue

When a message arrives, the API connects to an SMTP mail server in order to deliver an email message

<br/>

# Service Dependencies

This API relies on **SMTP** server for sending email messages 


<br/>

# How to start it up

If you have dotnet core SDK 3.1 installed you can just start up the API by running the following command:
```shell
dotnet run --project src/api.core.mail.csproj
```

If you don't make sure you have the docker installed and run the following commands:
```shell
docker build . -t api.core.mail
docker run api.core.mail -p 8000:8080
```
The project is set to run on port **5002** by default
you can change the default port in [src/Properties/launchSettings.json](src/Properties/launchSettings.json).
However if you start the API using docker, the API will run on port 8080.

# Possible troubles during the startup

All exceptions are being handled and the logs are being sent into SEQ server (https://datalust.co/)
Anyway, the following issues can make you feel confusing if you are not familiar with the project.

## RabbitMQ connection

A rabbit RabbitMQ cluster can take a while to initialize and since the API relies on it, when you start the API it might look "hanged"

If that happens it means that the API can't reach the RabbitMQ cluster so it looping attempting create the connection

Take a look at [src/appsettings.json](src/appsettings.json) configuration file in order to check the server hostname/port .

# Swagger Documentation

You can check de API documentation by browsing the following address
http://localhost:8000/swagger/index.html

*Please consider that the remote port might be different depending on your start up settings.*
