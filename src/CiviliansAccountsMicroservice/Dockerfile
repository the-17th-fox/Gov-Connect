FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY CiviliansAccountsMicroservice/*.sln ./

COPY CiviliansAccountsMicroservice/Civilians.Core/ ./Civilians.Core/
COPY CiviliansAccountsMicroservice/Civilians.Infrastructure/ ./Civilians.Infrastructure/
COPY CiviliansAccountsMicroservice/Civilians.Api/ ./Civilians.Api/

RUN dotnet publish Civilians.Api/Civilians.Api.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Civilians.Api.dll"]
