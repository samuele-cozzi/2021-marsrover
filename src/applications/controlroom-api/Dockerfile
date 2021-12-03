#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["applications/controlroom-api/controlroom-api.csproj", "applications/controlroom-api/"]
COPY ["applications/rover-iot/rover-iot.csproj", "applications/rover-iot/"]
COPY ["modules/rover/rover.domain/rover.domain.csproj", "modules/rover/rover.domain/"]
COPY ["modules/rover/rover.unittests/rover.unittests.csproj", "modules/rover/rover.unittests/"]
COPY ["modules/rover/rover.infrastructure.ef/rover.infrastructure.ef.csproj", "modules/rover/rover.infrastructure.ef/"]
COPY ["modules/rover/rover.infrastructure.rabbitmq/rover.infrastructure.rabbitmq.csproj", "modules/rover/rover.infrastructure.rabbitmq/"]
COPY ["rover.sln", "rover.sln"]
RUN dotnet restore "rover.sln"
COPY . .
WORKDIR "/src/applications/controlroom-api"
RUN dotnet build "controlroom-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "controlroom-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "controlroom-api.dll"]