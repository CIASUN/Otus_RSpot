# Otus_RSpot
Умная система бронирования рабочих мест

Функции:
Организации публикуют свои рабочие места с параметрами
Пользователи ищут места по фильтрам
Возможность подписки на «освобождение места»
Бронирование места

## 📦 Архитектура

- **UserService** — аутентификация, роли
- **PlaceService** — управление местами и организациями
- **BookingService** — бронирование
- **NotificationService** — уведомления через EventBus
- **React Frontend** — клиентское SPA

## 🛠 Технологии

- ASP.NET Core 8, EF Core, MongoDB, PostgreSQL
- React + Vite
- RabbitMQ, Serilog, Swagger, Docker, CI
- Microservices + EventBus

## 🚀 Быстрый запуск

1. Установи зависимости:
    ```bash
    dotnet restore
    cd frontend/rspot-react
    npm install
    ```

2. Запусти через Docker:
    ```bash
    docker-compose up --build
    ```

3. Открой в браузере:
    - Backend Swagger: http://localhost:5001/swagger
    - Frontend: http://localhost:3000

## ✅ CI

- GitHub Actions проверяет сборку, тесты и линт
