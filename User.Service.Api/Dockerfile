
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY User.Service.Api/*.csproj User.Service.Api/
COPY User.Servic.Business/*.csproj User.Servic.Business/
COPY User.Service.Core/*.csproj User.Service.Core/
COPY User.Service.DataAccess/*.csproj User.Service.DataAccess/

RUN dotnet restore User.Service.Api/User.Service.Api.csproj

COPY User.Service.Api/ User.Service.Api/
COPY User.Servic.Business/ User.Servic.Business/
COPY User.Service.Core/ User.Service.Core/
COPY User.Service.DataAccess/ User.Service.DataAccess/

RUN dotnet publish User.Service.Api/User.Service.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "User.Service.Api.dll"]