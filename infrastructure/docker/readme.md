# How the it works

This solution has the propose of creating local development environment so apis can be tested and makes the environment setup much easier.

The architecture of this solution is based of the following:

    |- nginx-proxy
        |- api-public-subscription
            |- api-core-subscription
            |- rabbit-mq-cluster
            |- api-core-email
        |- SEQ log server
        |- RabbitMQ manager

The docker network is behind and ngix-proxy which routes the incomming requests only thru the public api.

If the services behind the proxy aren't exposed via "port forwarding" they can't be reached outside the network. However in the internal network they are able to communicate with each others

The different components placed behind the gateway can be accessed using the following URLs:

http://localhost:8080/mq
http://localhost:8080/seq
http://localhost:8080/api/subscriptions


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
