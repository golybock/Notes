create table if not exists tag
(
    id   serial
        primary key,
    name varchar(100)
        unique
);

create table if not exists note_user
(
    id            integer default nextval('user_id_seq'::regclass) not null
        constraint user_pkey
            primary key,
    email         varchar(500)                                     not null
        constraint user_email_key
            unique,
    password_hash text,
    name          varchar(500)
);

create table if not exists note
(
    id            serial
        primary key,
    header        varchar(250)             not null,
    creation_date timestamp with time zone not null,
    edited_date   timestamp with time zone not null,
    source_path   text,
    user_id       integer
        references note_user,
    guid          uuid                     not null
        unique
);

create table if not exists note_tag
(
    id      serial
        primary key,
    note_id integer
        references note,
    tag_id  integer
        references tag
);

create table if not exists tokens
(
    id            serial
        primary key,
    user_id       integer
        references note_user,
    token         text                                   not null,
    refresh_token text                                   not null,
    creation_date timestamp with time zone default now() not null,
    ip            inet,
    active        boolean                  default true
);

