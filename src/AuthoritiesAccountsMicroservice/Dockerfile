FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ../AuthoritiesAccountsMicroservice/*.sln ./AuthoritiesAccountsMicroservice/

COPY ../AuthoritiesAccountsMicroservice/Authorities.Core/ ./AuthoritiesAccountsMicroservice/Authorities.Core/
COPY ../AuthoritiesAccountsMicroservice/Authorities.Application/ ./AuthoritiesAccountsMicroservice/Authorities.Application/
COPY ../AuthoritiesAccountsMicroservice/Authorities.Infrastructure/ ./AuthoritiesAccountsMicroservice/Authorities.Infrastructure/
COPY ../AuthoritiesAccountsMicroservice/Authorities.Api/ ./AuthoritiesAccountsMicroservice/Authorities.Api/

COPY ../SharedLib/SharedLib.ExceptionsHandler/ ./SharedLib/SharedLib.ExceptionsHandler/

RUN dotnet publish AuthoritiesAccountsMicroservice/Authorities.Api/Authorities.Api.csproj -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Authorities.Api.dll"]
