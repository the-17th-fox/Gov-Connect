FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY CommunicationsMicroservice/*.sln ./

COPY CommunicationsMicroservice/Communications.Core/ ./Communications.Core/
COPY CommunicationsMicroservice/Communications.Application/ ./Communications.Application/
COPY CommunicationsMicroservice/Communications.Hangfire/ ./Communications.Hangfire/
COPY CommunicationsMicroservice/Communications.Infrastructure/ ./Communications.Infrastructure/
COPY CommunicationsMicroservice/Communications.SignalR/ ./Communications.SignalR/
COPY CommunicationsMicroservice/Communications.Api/ ./Communications.Api/

COPY SharedLib/SharedLib.Kafka/ ./SharedLib.Kafka/
COPY SharedLib/SharedLib.Redis/ ./SharedLib.Redis/
COPY SharedLib/SharedLib.ExceptionsHandler/ ./SharedLib.ExceptionsHandler/

RUN dotnet publish Communications.Api/Communications.Api.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Communications.Api.dll"]
