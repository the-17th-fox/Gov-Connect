FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ../CommunicationsMicroservice/*.sln ./CommunicationsMicroservice/

COPY ../CommunicationsMicroservice/Communications.Core/ ./CommunicationsMicroservice/Communications.Core/
COPY ../CommunicationsMicroservice/Communications.Application/ ./CommunicationsMicroservice/Communications.Application/
COPY ../CommunicationsMicroservice/Communications.Hangfire/ ./CommunicationsMicroservice/Communications.Hangfire/
COPY ../CommunicationsMicroservice/Communications.Infrastructure/ ./CommunicationsMicroservice/Communications.Infrastructure/
COPY ../CommunicationsMicroservice/Communications.SignalR/ ./CommunicationsMicroservice/Communications.SignalR/
COPY ../CommunicationsMicroservice/Communications.Api/ ./CommunicationsMicroservice/Communications.Api/

COPY ../SharedLib/SharedLib.Kafka/ ./SharedLib/SharedLib.Kafka/
COPY ../SharedLib/SharedLib.Redis/ ./SharedLib/SharedLib.Redis/
COPY ../SharedLib/SharedLib.ExceptionsHandler/ ./SharedLib/SharedLib.ExceptionsHandler/
COPY ../SharedLib/SharedLib.ElasticSearch/ ./SharedLib/SharedLib.ElasticSearch/

RUN dotnet publish CommunicationsMicroservice/Communications.Api/Communications.Api.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Communications.Api.dll"]
