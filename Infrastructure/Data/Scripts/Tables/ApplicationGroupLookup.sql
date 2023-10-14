-- Table: lookups.ApplicationGroupLookup

-- DROP TABLE lookups."ApplicationGroupLookup";

CREATE TABLE lookups."ApplicationGroupLookup"
(
    "GroupId" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 67843 MINVALUE 67843 MAXVALUE 2147483647 CACHE 1 ),
    "GroupName" character varying(100) COLLATE pg_catalog."default",
    "Description" character varying(200) COLLATE pg_catalog."default",
    "CreatedDate" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    "ModifiedDate" timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "ApplicationGroup_pkey" PRIMARY KEY ("GroupId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE lookups."ApplicationGroupLookup"
    OWNER to postgres;