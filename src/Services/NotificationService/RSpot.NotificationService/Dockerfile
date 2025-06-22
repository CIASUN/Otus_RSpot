# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем .csproj
COPY src/Services/NotificationService/RSpot.NotificationService/RSpot.NotificationService.csproj RSpot.NotificationService/
RUN dotnet restore RSpot.NotificationService/RSpot.NotificationService.csproj

# Копируем весь исходный код
COPY . .

# Сборка и публикация
WORKDIR /src/src/Services/NotificationService/RSpot.NotificationService
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RSpot.NotificationService.dll"]
