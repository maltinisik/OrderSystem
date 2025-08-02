# SDK image - build için
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Projeyi kopyala ve restore et
COPY . .

# Restore ve Publish işlemleri
RUN dotnet restore
RUN dotnet publish OrderSystem.API/OrderSystem.API.csproj -c Release -o /app/publish

# 2. Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrderSystem.API.dll"]
