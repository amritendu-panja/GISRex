-- Table: public.SecurityTokenLog

-- DROP TABLE public."SecurityTokenLog";

CREATE TABLE public."SecurityTokenLog"
(
    "LogId" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "Token" character varying(4000) COLLATE pg_catalog."default" NOT NULL,
    "TokenType" smallint NOT NULL,
    "UserId" bigint NOT NULL,
    "IsEnabled" boolean NOT NULL,
    "CreateDate" timestamp with time zone NOT NULL,
    "ExpirationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "SecurityTokenLog_pkey" PRIMARY KEY ("LogId"),
    CONSTRAINT "FK_SecurityTokenLog_ApplicationUser" FOREIGN KEY ("UserId")
        REFERENCES public."ApplicationUser" ("UserId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."SecurityTokenLog"
    OWNER to postgres;