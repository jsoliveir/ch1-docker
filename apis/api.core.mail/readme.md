# Private API (api.core.mail)

This API is basically a consumer of a RabbitMQ message broker and it stands for email processing.

When RabbitMQ delivers an event to the right queue (see the appsettings.json) the API receives that event, creates an email message and sends it to an SMTP server.

The API was designed to be set in a private network and behind a secured gateway. 
It is fully opened and doesn't support authentication validations for performance improving. So it's assumed that this API is secured by gateways and private networking.


## Email sending

The API connects to RabbitMQ cluster and waits for an event message came from a specific queue

When a message arrives, the API connects to an SMTP mail server in order to deliver an email message

<br/>

# Service Dependencies

This API relies on **SMTP** server for sending email messages 


<br/>

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
Since the API relies on it, if the cluster is down or still initializing, when you start the API it might look "hanged"

If that happens, the API is looping, attempting to establish a connection to the RabbitMQ cluster but it can reach the cluster remote port.


So what to do:

>Take a look at [src/appsettings.json](src/appsettings.json) configuration file and check the cluster hostname and remote port.

>Check if the RabbitMQ is up and running

>Check if the configured port are exposed in the containers