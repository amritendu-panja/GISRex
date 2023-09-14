-- Table: public.ApplicationLogs

-- DROP TABLE public."ApplicationLogs";

CREATE TABLE public."ApplicationLogs"
(
    "LogId" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "Message" character varying(200) COLLATE pg_catalog."default",
    "LogLevel" character varying(50) COLLATE pg_catalog."default",
    "CreateDate" timestamp with time zone,
    host character varying(100) COLLATE pg_catalog."default",
    "IPAddress" character varying(50) COLLATE pg_catalog."default",
    "RequestUrl" character varying(500) COLLATE pg_catalog."default",
    "RequestHost" character varying(100) COLLATE pg_catalog."default",
    "RequestIPAddress" character varying(50) COLLATE pg_catalog."default",
    "ControllerName" character varying(150) COLLATE pg_catalog."default",
    "MethodName" character varying(150) COLLATE pg_catalog."default",
    CONSTRAINT "ApplicationLogs_pkey" PRIMARY KEY ("LogId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ApplicationLogs"
    OWNER to postgres;