# Используем образ .NET 8 для сборки и запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем проекты
COPY ["src/Services/NotificationService/RSpot.Notification.WorkerService/RSpot.Notification.WorkerService.csproj", "src/Services/NotificationService/RSpot.Notification.WorkerService/"]
COPY ["src/Services/NotificationService/RSpot.Notification.Infrastructure/RSpot.Notification.Infrastructure.csproj", "src/Services/NotificationService/RSpot.Notification.Infrastructure/"]

# Восстановление зависимостей
RUN dotnet restore "src/Services/NotificationService/RSpot.Notification.WorkerService/RSpot.Notification.WorkerService.csproj"

# Копируем остальной код
COPY . .

WORKDIR "/src/src/Services/NotificationService/RSpot.Notification.WorkerService"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RSpot.Notification.WorkerService.dll"]
