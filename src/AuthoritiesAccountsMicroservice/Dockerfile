FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY AuthoritiesAccountsMicroservice/*.sln ./

COPY AuthoritiesAccountsMicroservice/Authorities.Core/ ./Authorities.Core/
COPY AuthoritiesAccountsMicroservice/Authorities.Application/ ./Authorities.Application/
COPY AuthoritiesAccountsMicroservice/Authorities.Infrastructure/ ./Authorities.Infrastructure/
COPY AuthoritiesAccountsMicroservice/Authorities.Api/ ./Authorities.Api/

COPY SharedLib/SharedLib.ExceptionsHandler/ ./SharedLib.ExceptionsHandler/

RUN dotnet publish Authorities.Api/Authorities.Api.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Authorities.Api.dll"]
