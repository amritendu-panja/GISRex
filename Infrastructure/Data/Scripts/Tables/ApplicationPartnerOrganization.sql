-- Table: public.ApplicationPartnerOrganization

-- DROP TABLE public."ApplicationPartnerOrganization";

CREATE TABLE public."ApplicationPartnerOrganization"
(
    "OrganizationId" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    "UserId" bigint,
    "OrganizationName" character varying(200) COLLATE pg_catalog."default",
    "AddressLine1" character varying(200) COLLATE pg_catalog."default",
    "AddressLine2" character varying(200) COLLATE pg_catalog."default",
    "City" character varying(200) COLLATE pg_catalog."default",
    "StateCode" integer,
    "CountryCode" character varying(3) COLLATE pg_catalog."default",
    "PostCode" character varying(10) COLLATE pg_catalog."default",
    "CreateDate" timestamp without time zone,
    "ModifiedDate" timestamp without time zone,
    "Description" character varying(200) COLLATE pg_catalog."default",
    "LogoUrl" character varying(200) COLLATE pg_catalog."default",
    "Phone" character varying(20) COLLATE pg_catalog."default",
    CONSTRAINT "ApplicationPartnerOrganization_pkey" PRIMARY KEY ("OrganizationId"),
    CONSTRAINT "FK_Partner_User" FOREIGN KEY ("UserId")
        REFERENCES public."ApplicationUser" ("UserId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ApplicationPartnerOrganization"
    OWNER to postgres;