create table if not exists tag
(
    id   serial
        primary key,
    name varchar(100)
        unique
);

create table if not exists users
(
    id            serial       not null
        primary key,
    email         varchar(500) not null
        constraint user_email_key
            unique,
    password_hash text,
    name          varchar(500)
);

create table if not exists tokens
(
    id            serial
        primary key,
    user_id       integer
        references users,
    token         text                                   not null,
    refresh_token text                                   not null,
    creation_date timestamp with time zone default now() not null,
    ip            inet,
    active        boolean                  default true
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
    id            uuid                     not null
        primary key,
    header        varchar(250)             not null,
    creation_date timestamp with time zone not null,
    edited_date   timestamp with time zone not null,
    source_path   text,
    type_id       integer
        references note_type,
    owner_id      integer
        references users
);

create table if not exists note_tag
(
    id      serial
        primary key,
    tag_id  integer
        references tag,
    note_id uuid
        references note (id)
);

create table if not exists shared_notes
(
    id                   serial
        primary key,
    note_id              uuid
        references note (id),
    user_id              integer
        references users,
    permissions_level_id integer
        references permissions_level
);

