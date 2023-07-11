# Notes

Сервис заметок, реализованный в виде сайта

Использована монолитная архитектура приложения

### Стек:
- Asp.Net Core 7 (REST)
- PostgreSQL
- React

### Features
- Custom auth scheme (Refresh tokens + cookie)
- Full Async

Схема взаимеодействия компонентов:

![scheme](https://i.imgur.com/wRjLoh5.png)

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

## Frontend

Frontend представляет собой One-Page Application, написан на react, 
с использованием компонентов в виде классов

Главная страница приложения:

![mainPage](https://i.imgur.com/BZalMnf.png)

Страница авторизации:

![signInPage](https://i.imgur.com/8qi6F72.png)

Неудачная попытка авторизации:

![failSignInPage](https://i.imgur.com/0i8l16H.png)

Страница авторизации с введенными данными:

![dataSignInPage](https://i.imgur.com/rlaIjrO.png)

Страница регистрации:

![signUpPage](https://i.imgur.com/ml2IHAu.png)

Неудачная попытка регистрации:
![failSignUpPage](https://i.imgur.com/eEc2ags.png)

Страница заметки:

![notePage](https://i.imgur.com/a9vGNtY.png)

Диалог создания заметки:

![createNoteDialog](https://i.imgur.com/j1DKAL6.png)

![noteTagsDialog](https://i.imgur.com/GbfmV3t.png)