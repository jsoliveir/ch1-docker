# Public API (api.client.subscriptions)

This api handles public requests for a subscriptions prove of concept.
It acts like a back-end for frontend mostly and is ready to be set up behing a gateway.
The api supports tokens authentication/authorization, howerver for complexity reducing reasons, the tokens aren't being validated.

# Service Dependencies

This API depends on	api.core.subscriptions to run properly

# How to start it up (StandAlone)

If you have dotnet core SDK 3.1 installed you can just start up the API by running the following command:
```shell
	dotnet run --project ./src/api.client.subscriptions.csproj
```

If you don't make sure you have the docker installed and run the following commmands:
```shell
	docker build . -t api.client.subscriptions
	docker run api.client.subscriptions -p 8000:8080
```
The project is set to run on port 5002 by default
you can change the default port in Properties/launchSettings.json

however if you start the API using docker, the API will run on port 8080.


# Swagger Documentation

You can check de API documentation by browing the following address
http://localhost:8000/swagger/index.html

Please consider that the remote port might be different depending on your start up settings.

# Authentication

The api need an authorization header to be requested, however the authentication layer does not validate any kind of security tokens.
Since this API was designed for a prove of concept use the "Authorize" button in Swagger UI (Swagger Documentation) to specity a hipothetical security token.
You can check the authentication handler in src/Authentication/ApiKeyAuthenticationHandler.cs.


Once you've specidied the "hipothetical" security token you will be able to use the API.

