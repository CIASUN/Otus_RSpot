# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# �������� .csproj � ��������������� �����������
COPY src/Services/PlaceService/RSpot.Places.API/RSpot.Places.API.csproj ./RSpot.Places.API/
RUN dotnet restore ./RSpot.Places.API/RSpot.Places.API.csproj

# �������� �� ���������
COPY . .

# ��������� ������
WORKDIR /src/src/Services/PlaceService/RSpot.Places.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RSpot.Places.API.dll"]