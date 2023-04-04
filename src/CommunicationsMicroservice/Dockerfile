FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY CommunicationsMicroservice/*.sln ./

COPY CommunicationsMicroservice/Core/ ./Core/
COPY CommunicationsMicroservice/Infrastructure/ ./Infrastructure/
COPY CommunicationsMicroservice/Web/ ./Web/

RUN dotnet publish Web/Web.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Web.dll"]
