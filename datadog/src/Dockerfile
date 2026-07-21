FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copia o .NET Tracer do Datadog da imagem oficial de instrumentacao
COPY --from=datadog/dd-lib-dotnet-init:latest /datadog-init/monitoring-home /opt/datadog

COPY --from=build /app .
ENTRYPOINT ["dotnet", "ObservabilityDemo.Api.dll"]
