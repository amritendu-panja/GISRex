-- Table: public.ApplicationLayer

-- DROP TABLE public."ApplicationLayer";

CREATE TABLE public."ApplicationLayer"
(
    "LayerId" uuid NOT NULL,
    "LayerName" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    "FilePath" character varying(300) COLLATE pg_catalog."default",
    "OwnerId" bigint NOT NULL,
    "CreateDate" timestamp with time zone,
    "ModifiedDate" timestamp with time zone,
    CONSTRAINT "ApplicationLayer_pkey" PRIMARY KEY ("LayerId"),
    CONSTRAINT "ApplicationLayer_OwnerId_fkey" FOREIGN KEY ("OwnerId")
        REFERENCES public."ApplicationUser" ("UserId") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."ApplicationLayer"
    OWNER to postgres;