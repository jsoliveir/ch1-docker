#!/bin/sh

/usr/local/bin/nginx-prometheus-exporter\
    -web.listen-address=0.0.0.0:9113    \
    -nginx.scrape-uri http://127.0.0.1/metrics;

/docker-entrypoint.sh "nginx" "-g" "daemon off;"
