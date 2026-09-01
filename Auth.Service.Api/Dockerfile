
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Auth.Service.Api/*.csproj Auth.Service.Api/
COPY Auth.Service.Business/*.csproj Auth.Service.Business/
COPY Auth.Service.Core/*.csproj Auth.Service.Core/
COPY Auth.Service.DataAccess/*.csproj Auth.Service.DataAccess/

RUN dotnet restore Auth.Service.Api/Auth.Service.Api.csproj

COPY Auth.Service.Api/ Auth.Service.Api/
COPY Auth.Service.Business/ Auth.Service.Business/
COPY Auth.Service.Core/ Auth.Service.Core/
COPY Auth.Service.DataAccess/ Auth.Service.DataAccess/

RUN dotnet publish Auth.Service.Api/Auth.Service.Api.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Auth.Service.Api.dll"]