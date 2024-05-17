FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ../Gateway/*.sln ./Gateway/

COPY ../Gateway/Gateway/ ./Gateway/Gateway/

RUN dotnet publish Gateway/Gateway/Gateway.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Gateway.dll"]
