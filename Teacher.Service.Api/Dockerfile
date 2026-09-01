
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Teacher.Service.Api/*.csproj Teacher.Service.Api/
COPY Teacher.Service.Business/*.csproj Teacher.Service.Business/
COPY Teacher.Service.Core/*.csproj Teacher.Service.Core/
COPY Teacher.Service.DataAccess/*.csproj Teacher.Service.DataAccess/

RUN dotnet restore Teacher.Service.Api/Teacher.Service.Api.csproj

COPY Teacher.Service.Api/ Teacher.Service.Api/
COPY Teacher.Service.Business/ Teacher.Service.Business/
COPY Teacher.Service.Core/ Teacher.Service.Core/
COPY Teacher.Service.DataAccess/ Teacher.Service.DataAccess/

RUN dotnet publish Teacher.Service.Api/Teacher.Service.Api.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Teacher.Service.Api.dll"]