-- Table: lookups.PoliticalStateLookup

-- DROP TABLE lookups."PoliticalStateLookup";

CREATE TABLE lookups."PoliticalStateLookup"
(
    "StateUniqueId" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "CountryCode" character varying(3) COLLATE pg_catalog."default",
    "StateId" integer,
    "StateCode" character varying(10) COLLATE pg_catalog."default",
    "StateName" character varying(100) COLLATE pg_catalog."default",
    type_1 character varying(50) COLLATE pg_catalog."default",
    engtype_1 character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "PoliticalStateLookup_pkey" PRIMARY KEY ("StateUniqueId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE lookups."PoliticalStateLookup"
    OWNER to postgres;