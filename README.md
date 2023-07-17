# Notes

Сервис заметок, реализованный в виде сайта

Использована монолитная архитектура приложения

### Стек:
- Asp.Net Core 7 (REST)
- PostgreSQL
- React
- Redis

### Features
- Custom auth scheme (Refresh tokens + cookie)
- Full Async
- Caching tokens

Схема взаимеодействия компонентов:

![scheme](https://imgur.com/Iwl3oWB)

## Backend

Взаимодействие с базой происходит через ADO.NET

Модели разделены на уровни (и разные библиотеки):
- Blank
- Database
- Domain
- View

Для создания объектов созданы классы паттерна builder, для 
каждого из уровней моделей

Переход состояний модели происходит следующим образом:
(blank-database) или (database-domain-view)

Структура проекта:

В основе лежит паттерн Repository-Service, с вынесением логики работы с базой
в отдельные классы

Репозитории для работы с базой вынесены в отдельную библиотеку,
для возможности смены "основного" блока приложения (REST API)

Все библиотеки подключены к основному проекту,
вся структура представлена на изображении ниже

![structure](https://i.imgur.com/pOOyx0J.png)
