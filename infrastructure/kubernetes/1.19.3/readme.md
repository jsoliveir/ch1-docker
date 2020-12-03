# How to deploy into Kubernetes Cluster

For demo purposes the public gateway is exposed using a NodePort service
[gateways/public-gateway/service.yml](gateways/public-gateway/service.yml)
```shell
	kubectl kustomize  "infrastructure/kubernetes/1.19.3/" | kubectl apply -f -
```

Given that the public gateway is exposed on NodePort 30000 these are the URLs available :
http://localhost:30000/public/subscriptions/swagger/index.html
http://localhost:30000/private/mq/
http://localhost:30000/private/seq/
http://localhost:30000/private/smtp/

In a production environment with multiple nodes (VM) the public gateway would be exposed thru an ingress controller or thru a LoadBalance service.
