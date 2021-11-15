#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["applications/controlroom.api/controlroom.api/controlroom.api.csproj", "applications/controlroom.api/controlroom.api/"]
COPY ["applications/rover/rover/rover.csproj", "applications/rover/rover/"]
COPY ["modules/rover/rover.domain/rover.domain.csproj", "modules/rover/rover.domain/"]
COPY ["modules/rover/rover.unittest/rover.unittest.csproj", "modules/rover/rover.unittest/"]
COPY ["modules/rover/rover.infrastructure.ef/rover.infrastructure.ef.csproj", "modules/rover/rover.infrastructure.ef/"]
COPY ["modules/rover/rover.infrastructure.rabbitmq/rover.infrastructure.rabbitmq.csproj", "modules/rover/rover.infrastructure.rabbitmq/"]
COPY ["rover.sln", "rover.sln"]
RUN dotnet restore "rover.sln"
COPY . .
WORKDIR "/src/applications/rover/rover"
RUN dotnet build "rover.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rover.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rover.dll"]
