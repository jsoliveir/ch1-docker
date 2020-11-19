# Microservices

In this directory you cam see the following microservices:
	- api.client.subscriptions
	- api.core.subcriptions
	- api.mail.subscriptions

# api.client.subscriptions
[api.client.subscriptions/](api.client.subscriptions/readme.md)
	- It is a dotnet core API 
	- It was designed to be publicly available.
	- It supports security tokens authorization
	- It performs CRUD operations against private core.services

# api.core.subscriptions
[api.core.subscriptions/](api.core.subscriptions/readme.md)
api.core.subscriptions/
	- It is a dotnet core API 
	- It was designed to be placed in a private network and behind gateway
	- It was designed for processing CRUD operations as fast as possible against a database.
	- It is fully opened and do not support authentication
	- It relies on RabbitMQ for processing mailing event messages 

# api.core.mail
[api.core.mail/](api.core.mail/readme.md)
	- It is a dotnet core API 
	- It was designed to be placed in a private network and behind gateway
	- It was designed to be a consumer and process event messages coming from RabbitMQ.
	- It is fully opened and do not support authentication
	- It relies on SMTP server for delivering emails based on received RabbitMQ event messages 


