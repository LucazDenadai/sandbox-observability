FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Instala o .NET Tracer do Datadog para auto-instrumentacao via CLR Profiler
RUN apt-get update && apt-get install -y curl && \
    curl -LO https://github.com/DataDog/dd-trace-dotnet/releases/latest/download/datadog-dotnet-apm_amd64.deb && \
    dpkg -i ./datadog-dotnet-apm_amd64.deb && \
    rm datadog-dotnet-apm_amd64.deb && \
    apt-get purge -y curl && apt-get autoremove -y && rm -rf /var/lib/apt/lists/*

COPY --from=build /app .
ENTRYPOINT ["dotnet", "ObservabilityDemo.Api.dll"]
