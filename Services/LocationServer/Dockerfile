FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine as builder
WORKDIR /app
COPY . .
RUN dotnet restore && \
    dotnet build && \
    dotnet publish -c release -r linux-musl-x64 -o /locationserver

# locationservice

FROM alpine:3.8

# # RUN GRPC_HEALTH_PROBE_VERSION=v0.2.0 && \
# #     wget -qO/bin/grpc_health_probe https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 && \
# #     chmod +x /bin/grpc_health_probe

# Dependencies for runtime
# busybox-extras => telnet
RUN apk update && apk add --no-cache \
    busybox-extras \
    libc6-compat \
    libunwind \
    libuuid \
    libgcc \
    libstdc++ \
    libintl \
    icu


WORKDIR /app
COPY --from=builder /locationserver .
ENTRYPOINT ["./LocationServer", "start"]
