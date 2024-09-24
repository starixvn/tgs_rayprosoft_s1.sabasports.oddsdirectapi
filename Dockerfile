FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS build
WORKDIR /src
COPY ["S1.SabaSports.OddsDirectAPI/S1.SabaSports.OddsDirectAPI.csproj", "S1.SabaSports.OddsDirectAPI/"]
RUN dotnet restore ./S1.SabaSports.OddsDirectAPI/S1.SabaSports.OddsDirectAPI.csproj
COPY . .
WORKDIR "/src/S1.SabaSports.OddsDirectAPI"
RUN dotnet build "./S1.SabaSports.OddsDirectAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./S1.SabaSports.OddsDirectAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM hub.lumigame.com/base/dotnet/core/aspnet:8.0-alpine-24h1.1 AS base
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S1.SabaSports.OddsDirectAPI.dll"]