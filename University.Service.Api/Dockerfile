
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY University.Service.Api/*.csproj University.Service.Api/
COPY University.Service.Business/*.csproj University.Service.Business/
COPY University.Service.Core/*.csproj University.Service.Core/
COPY University.Service.DataAccess/*.csproj University.Service.DataAccess/

RUN dotnet restore University.Service.Api/University.Service.Api.csproj

COPY University.Service.Api/ University.Service.Api/
COPY University.Service.Business/ University.Service.Business/
COPY University.Service.Core/ University.Service.Core/
COPY University.Service.DataAccess/ University.Service.DataAccess/

RUN dotnet publish University.Service.Api/University.Service.Api.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "University.Service.Api.dll"]