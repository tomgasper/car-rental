FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CarRental.sln", "./"]
COPY ["src/CarRental.csproj", "src/"]
COPY ["tests/WebApiTests/WebApiTests.csproj", "tests/WebApiTests/"]

RUN dotnet restore

COPY . .

RUN dotnet publish "src/CarRental.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CarRental.dll"]