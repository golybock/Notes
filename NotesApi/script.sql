create table if not exists tag
(
    id   serial
        primary key,
    name varchar(100)
        unique
);

create table if not exists "user"
(
    id            serial
        primary key,
    email         varchar(500) not null
        unique,
    password_hash text,
    name          varchar(500)
);

create table if not exists note
(
    id             serial
        primary key,
    header         varchar(250)             not null,
    creation_date  timestamp with time zone not null,
    last_edit_date timestamp with time zone not null,
    source_path    text,
    user_id        integer
        references "user"
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

