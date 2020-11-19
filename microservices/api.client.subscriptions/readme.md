# Public API (api.client.subscriptions)

This API handles public requests for subscriptions Proof of Concept

It acts as a back-end for frontend mostly and is ready to be set up behind a gateway

The API supports tokens for authentication/authorization, however for complexity reducing reasons, the tokens aren't validated.

# Service Dependencies

This API depends on	api.core.subscriptions to run properly

# How to start it up (StandAlone)

If you have dotnet core SDK 3.1 installed you can just start up the API by running the following command:
```shell
dotnet run --project ./src/api.client.subscriptions.csproj
```

If you don't make sure you have the docker installed and run the following commands:
```shell
docker build . -t api.client.subscriptions
docker run api.client.subscriptions -p 8000:8080
```
The project is configured to run on port 5002 by default but it can changed in [./src/Properties/launchSettings.json](./src/Properties/launchSettings.json).

_When running the api inside a container the default port is set to 8080_

# Swagger Documentation

Once you start the API you can check de API documentation by browsing the following address:

[http://localhost:5002/swagger/](http://localhost:5002/swagger/)

Please consider that the remote port might be different depending on your startup settings. 

>The port 8080 is used when the API runs inside docker containers.

>Check the ASPNETCORE_URL environment varible inside the dockerfile before you go

[http://localhost:8080/swagger/](http://localhost:8080/swagger/)


# Authentication

Since this API was designed for a proof of concept, the API requires an auth token to be invoked but the token is not validated, so you can use any string as a valid token for the demo purpose 

on top of the swagger UI, there is an "Authorize" button where you can open a from and specify an hypothetical security token (use any string).

The authentication handler is over here:
[./src/Authentication/ApiKeyAuthenticationHandler.cs](./src/Authentication/ApiKeyAuthenticationHandler.cs)


