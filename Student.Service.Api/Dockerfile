FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Student.Service.Api/*.csproj Student.Service.Api/
COPY Student.Service.Business/*.csproj Student.Service.Business/
COPY Student.Service.Core/*.csproj Student.Service.Core/
COPY Student.Service.DataAccess/*.csproj Student.Service.DataAccess/

RUN dotnet restore Student.Service.Api/Student.Service.Api.csproj

COPY Student.Service.Api/ Student.Service.Api/
COPY Student.Service.Business/ Student.Service.Business/
COPY Student.Service.Core/ Student.Service.Core/
COPY Student.Service.DataAccess/ Student.Service.DataAccess/

RUN dotnet publish Student.Service.Api/Student.Service.Api.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Student.Service.Api.dll"]