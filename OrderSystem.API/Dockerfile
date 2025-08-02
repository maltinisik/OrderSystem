# SDK image - build için
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Projeyi kopyala ve restore et
COPY *.sln .
COPY OrderSystem.API/*.csproj ./OrderSystem.API/
RUN dotnet restore

# Projeyi build et
COPY . .
WORKDIR /src/OrderSystem.API
RUN dotnet publish -c Release -o /app/publish

# Runtime image - çalışma için
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderSystem.API.dll"]
