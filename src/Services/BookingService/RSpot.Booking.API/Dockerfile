# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# �������� .csproj � ��������������� �����������
COPY src/Services/BookingService/RSpot.Booking.API/RSpot.Booking.API.csproj ./RSpot.Booking.API/
RUN dotnet restore ./RSpot.Booking.API/RSpot.Booking.API.csproj

# �������� �� ���������
COPY . .

# ��������� ������
WORKDIR /src/src/Services/BookingService/RSpot.Booking.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RSpot.Booking.API.dll"]