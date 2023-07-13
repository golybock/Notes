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
    password_hash text,
    name          varchar(250)
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
    header       varchar(250)                           not null,
    created_date timestamp with time zone default now() not null,
    edited_date  timestamp with time zone default now() not null,
    id           uuid                                   not null
        primary key,
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

create table if not exists note_images
(
    id      uuid              not null
        primary key,
    note_id uuid              not null
        references note,
    x       integer default 0 not null,
    y       integer default 0 not null,
    width   integer default 1 not null,
    height  integer default 1 not null
);

