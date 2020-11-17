# Prove of concept

This repository contains infrastructure configurations and services in order to simulate a simple subscriptions system.

# What is in this repository


	infrastructure/			| contains all configurations needed for setting up the project infrastructure
		| docker /			| contains a docker-compose infrastructure solution for running the project locally.
		| kubernetes /		| contains a kubernetes infrastructure solution for running the project.
		| services/			| contains all sidecar services (log server, smtp, brokers, gateways)
			| gateways/		| 
				| public/	| contains configurations for set up a public gateway server 
				| private/	| contains configurations for set up a private internal gateway server
			| rabbitmq/		| 
				| etc/		| contains startup configurations for RabbitMQ message broker
				| lb/		| contains nginx configurations for load balancing rabbitMQ servers


# Solution architecture topology

The architecture of this project is based of the following topology:

   **|-> public-gateway                  (network:	 public)**
		|-> api.client.subscriptions     (network:   public | private)
			|-> api.core.subscriptions   (network:   private)
			|-> api.core.mail			 (network:   private)  
	  **|-> private-gateway				 (network:   public | private)**
			|-> RabbitMQ (cluster)       (network:   private)
				|-> rabbit-1			 (network:   private)
				|-> rabbit-2			 (network:   private)
			|-> SMTP server              (network:   private)
			|-> SEQ logs server          (network:   private)

# Explaining the differents components

There are two different types of networks public and private.

### Networking
The public network :
	- hosts public apis and applications that can must be reachable from the internet.

The private network:
	- hosts core services, the private and the most critial ones so to speak.

### Services
The public-gateway:
	- is placed in the public network
	- cannot reach the private services.
	- routes incomming requests thru:
		- the public api.client.subscriptions
		- the private-gateway
	- exposes internal services to the internet
	- access control
	- validate tokens 
	- authenticate endpoints
	- produce logs

The private gateway:
	- can reach services in the public network
	- can reach services in the private network 
	- increases control over security (when exposing dashboards)
	- increases control over monitoring
	- restricts access to the private network
	- exposes partial private services to the public network

The api.client.subscription: *more information in ./microservices/api.client.subscriptions*
	- stands for basically handling requests for subscriptions creation
	- can reach services in the public network
	- can reach services in the private network 

The RabbitMQ (cluster):
	- is an nginx load balancer
	- is composed by two rabbitMQ servers in cluster (rabbit-1, rabbit2)
	- only reachable inside the private network
	- provides dashboards messages queuing
	- provides a way of publishing new event messages

The SMTP server
	- is simple SMTP/Mail Inbox server
	- SMTP only reachable in the private network
	- Mail inbox reachable exposed by private/public gateways

The SEQ log server
	- is simple tool for monotoring logs produced by the API's.'


# Interesting Links

you will need to pass the basic gateway authentication in order to see the private links (dashboards)

basic auth  credentials:
	- username: admin
	- password:admin

RabbitMQ cluster manager (guest:guest): http://localhost:8080/private/mq
SEQ Logging dashboards                : http://localhost:8080/private/seq
Mail inbox dashboards                 : http://localhost:8080/private/mail
Public API						      : http://localhost:8080/public/subscriptions/swagger/

# How to start it up the solution

Set your console ***current working directory*** to the following repository path:
	./infrastructure/docker

```shell
    cd ./infrastructure/docker
```

**Clean up you docker set up**

_(shell script version)_
```shell
    #!/bin/sh
    rm -f  ~/.docker/config.json || true;
    docker-compose down
    docker system prune --all
    docker network prune -f
```

_(powershell version)_
```powershell
    Remove-Item -ErrorAction SilentlyContinue  ~/.docker/config.json 

    docker-compose down
    docker system prune --all
    docker network prune -f
```

**Start up the containers**

_(shell script version)_
```shell
     docker-compose up --force-recreate --remove-orphans;
```
_(powershell version)_
```powershell
    docker-compose up --force-recreate --remove-orphans
```

_(please note that rabbitmq cluster can take up to 2min to get initialized)_


Check if RabbitMQ is already up and running
 http://localhost:8080/mq/

 
Check if the API is up and running
http://localhost:8080/public/subscriptions/swagger/
