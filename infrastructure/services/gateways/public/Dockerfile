FROM golang:alpine  as prometheus
WORKDIR /usr/src
RUN apk add git make 
RUN git clone https://github.com/nginxinc/nginx-prometheus-exporter.git

WORKDIR /usr/src/nginx-prometheus-exporter/
RUN git checkout master
RUN git reset --hard f5a5f8086c19ba6a4c6eafe89acca806334fa427
RUN make

FROM nginx:alpine as serve

COPY docker-entrypoint.sh \
    /usr/docker-entrypoint.sh

COPY --from=prometheus \
    /usr/src/nginx-prometheus-exporter/nginx-prometheus-exporter \
    /usr/local/bin/nginx-prometheus-exporter

EXPOSE 8080
EXPOSE 9113

ENTRYPOINT ["/usr/docker-entrypoint.sh"]


