-- Table: public.ApplicationUser

-- DROP TABLE public."ApplicationUser";

CREATE TABLE public."ApplicationUser"
(
    "CreatedDate" timestamp with time zone,
    "ModifiedDate" timestamp with time zone,
    "PasswordEncrypted" character varying(200) COLLATE pg_catalog."default",
    "PasswordSalt" character varying(100) COLLATE pg_catalog."default",
    "UserGuid" uuid NOT NULL,
    "UserId" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "UserName" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "IsEnabled" boolean,
    "IsUserLocked" boolean NOT NULL,
    "Email" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "IsPasswordExpired" boolean NOT NULL,
    CONSTRAINT "ApplicationUser_pkey" PRIMARY KEY ("UserId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ApplicationUser"
    OWNER to postgres;