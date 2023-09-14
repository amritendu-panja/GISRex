-- Table: lookups.CountryLookup

-- DROP TABLE lookups."CountryLookup";

CREATE TABLE lookups."CountryLookup"
(
    "CountryId" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "CountryName" character varying(200) COLLATE pg_catalog."default",
    "CountryCode" character varying(2) COLLATE pg_catalog."default",
    "CallingCode" character varying(20) COLLATE pg_catalog."default",
    "TimeZone" character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT "CountryLookup_pkey" PRIMARY KEY ("CountryId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE lookups."CountryLookup"
    OWNER to postgres;