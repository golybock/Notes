create table if not exists tag
(
    id   uuid not null
        primary key,
    name varchar(100)
        unique
);

create table if not exists users
(
    id            uuid         not null
        constraint user_pkey
            primary key,
    email         varchar(500) not null
        constraint user_email_key
            unique,
    password_hash text
);

create table if not exists tokens
(
    id            serial
        primary key,
    user_id       uuid
        references users,
    token         text                                   not null,
    refresh_token text                                   not null,
    created_date  timestamp with time zone default now() not null,
    ip            inet
);

create table if not exists permissions_level
(
    id   serial
        primary key,
    name varchar(150)
);

create table if not exists note_type
(
    id   serial
        primary key,
    name varchar(150)
);

create table if not exists note
(
    id           uuid                                   not null
        primary key,
    header       varchar(250)                           not null,
    created_date timestamp with time zone default now() not null,
    edited_date  timestamp with time zone default now() not null,
    source_path  text                                   not null,
    type_id      integer
        references note_type,
    owner_id     uuid
        references users
);

create table if not exists note_tag
(
    id      serial
        primary key,
    tag_id  uuid
        references tag,
    note_id uuid
        references note
);

create table if not exists shared_notes
(
    id                   serial
        primary key,
    note_id              uuid
        references note,
    user_id              uuid
        references users,
    permissions_level_id integer
        references permissions_level
);

create table if not exists logs
(
    id        serial
        primary key,
    timestamp timestamp with time zone default now(),
    action    varchar(500) not null,
    user_id   uuid         not null
        references users,
    note_id   uuid
        references note
);

