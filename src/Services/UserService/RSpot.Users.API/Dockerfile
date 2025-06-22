# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем .csproj и восстанавливаем зависимости
COPY src/Services/UserService/RSpot.Users.API/RSpot.Users.API.csproj ./RSpot.Users.API/
RUN dotnet restore ./RSpot.Users.API/RSpot.Users.API.csproj

# Копируем всё остальное
COPY . .

# Публикуем проект
WORKDIR /src/src/Services/UserService/RSpot.Users.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RSpot.Users.API.dll"]
