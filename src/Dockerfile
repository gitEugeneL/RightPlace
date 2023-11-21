FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Domain/Domain.csproj", "src/Domain/"]
COPY ["Application/Application.csproj", "src/Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["Api/Api.csproj", "src/Api/"]

RUN dotnet restore "src/Api/Api.csproj"

COPY . . 

WORKDIR "/src/Api/"
RUN dotnet build -c Release -o /app/build


FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
