# Public API (api.client.subscriptions)

This API handles public requests for subscriptions Proof of Concept

It acts as a back-end for frontend mostly and is ready to be set up behind a gateway

The API supports tokens authentication/authorization, however for complexity reducing reasons, the tokens aren't being validated.

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
The project is set to run on port 5002 by default
you can change the default port in [./src/Properties/launchSettings.json](./src/Properties/launchSettings.json).
However if you start the API using docker, the API will run on port 8080.


# Swagger Documentation

You can check de API documentation by browsing the following address

[http://localhost:8000/swagger/](http://localhost:8000/swagger/)

Please consider that the remote port might be different depending on your start up settings.


# Authentication

The app need an authorization header to be requested, however the authentication layer does not validate any kind of security tokens

Since this API was designed for a proof of concept use the "Authorize" button in Swagger UI (Swagger Documentation) to specify a hypothetical security token

You can check the authentication handler in:
[./src/Authentication/ApiKeyAuthenticationHandler.cs](./src/Authentication/ApiKeyAuthenticationHandler.cs)

Once you've specified the "hypothetical" security token you will be able to use the API.

