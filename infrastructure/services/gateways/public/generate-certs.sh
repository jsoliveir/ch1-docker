# OpenSSL can be downloaded here:
#    https://slproweb.com/download/Win64OpenSSL-1_1_1h.msi

openssl req -x509 -nodes -days ExpireDays -newkey rsa:1024 \
    -keyout certs/private/certificate.key \
    -out certs/public/certificate.crt