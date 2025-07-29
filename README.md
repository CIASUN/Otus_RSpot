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

Микросервисная архитектура: 4 сервиса(NotificationService, BookingService, UserService, PlaceService)  БД: PostgreSQL(2шт) или NoSQL(MongoDB). RabbitMQ

Структурное логирование: через Serilog с ротацией файлов(NotificationService, BookingService) Логи отражаются в папку на хост машине.

API документация: с помощью Swagger/OpenAPI, автоматически генерируется из контроллеров(BookingService)

JWT-аутентификация: защищённый доступ к API

CORS: для безопасного взаимодействия фронтенда и бекенд-сервисов

Docker + docker-compose: 8 контейнеров, один из них фронт React(с маршрутизацией, авторизацией)+ Vite

CI/CD: автоматическая сборка и тестирование сервисов через GitHub Actions

Middleware логирование запросов (BookingService)

ASP.NET Core(.NET 8) EF Core

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

---

 
