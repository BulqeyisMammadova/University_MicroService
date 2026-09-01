
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ApiGateway/*.csproj ApiGateway/

RUN dotnet restore ApiGateway/ApiGateway.csproj

COPY ApiGateway/ ApiGateway/

RUN dotnet publish ApiGateway/ApiGateway.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]