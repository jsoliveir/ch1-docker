# Acrhitecture

This solution has the propose of creating local development environment so apis can be tested and makes the environment setup much easier.

The architecture of this solution is based of the following topology:

    |-> nginx-proxy                  (network:   public|tools|services)
      |-> api-public-subscription    (network:   private|public)
        |-> api-core-subscription    (network:   private)
        |-> api-core-email           (network:   private)     
      |-> RabbitMQ manager           (network:   private|services)
      |-> Mail server                (network:   private|tools)
      |-> SEQ server                 (network:   private|tools)

All the components are behind an nginx proxy.
The nginx proxy is responsible for routing the incomming requests to the public api and dashboards for the monitoring tools.
The other services are able to communicate with each others however they must be placed in the same network.
(if there is a need of testing a specific internal service, it can be done by setting a port forwading to the target service/container.

Since all services are only exposed thru the nginx proxy server, here is some links to accessed the different dashboards:

RabbitMQ cluster manager: http://localhost:8080/mq
SEQ Logging dashboards  : http://localhost:8080/seq
Mail inbox dashboards   : http://localhost:8080/mail
Public Subscriptions API: http://localhost:8080/api/subscriptions/swagger/


# Networking

There are 3 distinguish networks:
    * public
    * private
    * services
    * tools

The public network stands for hosting services that may have to be accessed from the outside (always thru an nginx gateway)
The private network stands for hosting private core services that must only be accessed via an internal gateway/pi (public-subscriptions)
The tools network stands for hosting tools like monitoring tools. Only the tools dashboards can be accessed thru the nginx gateway (via HTTP).
The service network stands for hosting services like message queue brokers or databases servers. Only dashboards can be accessed thru the nginx gateway (via HTTP).


# How to start it up

Set your console ***current working directory*** to this repository path
```shell
    cd infrastructure/docker
```

then run the following commands:

_(shell script version)_
```shell
    #!/bin/sh
    docker-compose rm -f;
    docker-compose pull;
    docker-compose build;

    rm -f  ~/.docker/config.json || true;
    
    docker-compose up \
        --force-recreate \
        --remove-orphans;
```

_(powershell version)_

```powershell
    docker-compose rm -f
    docker-compose pull
    docker-compose build

    Remove-Item `
        -ErrorAction SilentlyContinue `
        ~/.docker/config.json 

    docker-compose up `
        --force-recreate `
        --remove-orphans
```

_(please note that rabbitmq cluster can take up to 2min to get initialized)_
