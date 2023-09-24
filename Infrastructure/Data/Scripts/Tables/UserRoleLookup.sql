-- Table: lookups.UserRoleLookup

-- DROP TABLE lookups."UserRoleLookup";

CREATE TABLE lookups."UserRoleLookup"
(
    "RoleId" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "Role" character varying(30) COLLATE pg_catalog."default",
    "Description" character varying(100) COLLATE pg_catalog."default",
    CONSTRAINT "UserRoleLookup_pkey" PRIMARY KEY ("RoleId")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE lookups."UserRoleLookup"
    OWNER to postgres;