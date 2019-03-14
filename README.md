# Microservices

TODO


- .NET Core 2.2
- gRPC


### Location Service


- Build and run the server

  ```
  $ dotnet run -p Services\LocationServer\LocationServer
  ```

- Build and run the client

  ```
  $ dotnet run -p LocationClient\locationClient

#### Docker
```
$ cd Services\LocationServer
$ docker build -t aurlaw/microservices/locationservice .
$ docker run -it --name=aurlaw-micro-locationservice --rm -e LISTEN_ADDR=localhost -e PORT=7070 -p 7070:7070 aurlaw/microservices/locationservice

```