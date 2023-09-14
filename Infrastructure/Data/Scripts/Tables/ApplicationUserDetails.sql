-- Table: public.ApplicationUserDetails

-- DROP TABLE public."ApplicationUserDetails";

CREATE TABLE public."ApplicationUserDetails"
(
    "UserId" integer NOT NULL,
    "ImagePath" character varying(500) COLLATE pg_catalog."default",
    "FirstName" character varying(100) COLLATE pg_catalog."default",
    "LastName" character varying(100) COLLATE pg_catalog."default",
    "AddressLine1" character varying(200) COLLATE pg_catalog."default",
    "AddressLine2" character varying(200) COLLATE pg_catalog."default",
    "City" character varying(200) COLLATE pg_catalog."default",
    "StateCode" character varying(200) COLLATE pg_catalog."default",
    "PostCode" character varying(50) COLLATE pg_catalog."default",
    "Mobile" character varying(20) COLLATE pg_catalog."default",
    "AlternateEmail" character varying(200) COLLATE pg_catalog."default",
    "CreateDate" timestamp with time zone,
    "ModifiedDate" timestamp with time zone,
    "ContryCode" character varying(10) COLLATE pg_catalog."default",
    "AlternateMobile" character varying(20) COLLATE pg_catalog."default",
    CONSTRAINT "ApplicationUserDetails_pkey" PRIMARY KEY ("UserId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ApplicationUserDetails"
    OWNER to postgres;