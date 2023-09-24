-- Table: lookups.CountryLookup

-- DROP TABLE lookups."CountryLookup";

CREATE TABLE lookups."CountryLookup"
(
    "CountryId" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "CountryName" character varying(200) COLLATE pg_catalog."default",
    "CountryCode" character varying(2) COLLATE pg_catalog."default",
    "CallingCode" character varying(20) COLLATE pg_catalog."default",
    "TimeZone" character varying(50) COLLATE pg_catalog."default",
    "ISO3Code" character varying(3) COLLATE pg_catalog."default",
    "HasLevel1" boolean DEFAULT false,
    "HasLevel2" boolean DEFAULT false,
    "HasLevel4" boolean DEFAULT false,
    "HasLevel5" boolean DEFAULT false,
    "Level1Name" character varying(100) COLLATE pg_catalog."default",
    "Level2Name" character varying(100) COLLATE pg_catalog."default",
    "Level3Name" character varying(100) COLLATE pg_catalog."default",
    "Level4Name" character varying(100) COLLATE pg_catalog."default",
    "Level5Name" character varying(100) COLLATE pg_catalog."default",
    "HasLevel3" boolean DEFAULT false,
    "ISOCode" character varying(3) COLLATE pg_catalog."default",
    "Region" character varying(100) COLLATE pg_catalog."default",
    "SubRegion" character varying(100) COLLATE pg_catalog."default",
    "IntermediateRegion" character varying(100) COLLATE pg_catalog."default",
    "RegionCode" character varying(3) COLLATE pg_catalog."default",
    "SubRegionCode" character varying(3) COLLATE pg_catalog."default",
    "IntermediateRegionCode" character varying(3) COLLATE pg_catalog."default",
    CONSTRAINT "CountryLookup_pkey" PRIMARY KEY ("CountryId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE lookups."CountryLookup"
    OWNER to postgres;