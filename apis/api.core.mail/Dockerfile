FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as restore
WORKDIR /src
COPY src/*.csproj /src
COPY nuget.config /src
RUN dotnet restore

FROM restore as build
ARG CONFIGURATION
WORKDIR /src
COPY . /src

## build and publish
RUN dotnet publish src/ \
	--configuration ${CONFIGURATION:-"Release"} \
	--output /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine as setup
COPY --from=build /app /app
WORKDIR /app

RUN apk update				
RUN apk add --no-cache tzdata	# timezones            

## set security 
RUN addgroup -g 9999 appgroup 
RUN adduser -S -u 9999 -g appgroup appuser
RUN chown -R appuser:appgroup /app

#set the timezone norwegian
RUN echo "Europe/Lisbon" > /etc/timezone && \
cp /usr/share/zoneinfo/Europe/Oslo /etc/timezone

# serve the application
FROM setup as hosting
WORKDIR /app

EXPOSE 8080
USER appuser
ENV ASPNETCORE_URLS http://*:8080
ENTRYPOINT [ "dotnet","api.core.mail.dll"]