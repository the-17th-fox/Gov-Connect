FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ../CiviliansAccountsMicroservice/*.sln ./CiviliansAccountsMicroservice/

COPY ../CiviliansAccountsMicroservice/Civilians.Core/ ./CiviliansAccountsMicroservice/Civilians.Core/
COPY ../CiviliansAccountsMicroservice/Civilians.Application/ ./CiviliansAccountsMicroservice/Civilians.Application/
COPY ../CiviliansAccountsMicroservice/Civilians.Infrastructure/ ./CiviliansAccountsMicroservice/Civilians.Infrastructure/
COPY ../CiviliansAccountsMicroservice/Civilians.Api/ ./CiviliansAccountsMicroservice/Civilians.Api/

COPY ../SharedLib/SharedLib.ExceptionsHandler/ ./SharedLib/SharedLib.ExceptionsHandler/
COPY ../SharedLib/SharedLib.Kafka/ ./SharedLib/SharedLib.Kafka/

RUN dotnet publish CiviliansAccountsMicroservice/Civilians.Api/Civilians.Api.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Civilians.Api.dll"]
