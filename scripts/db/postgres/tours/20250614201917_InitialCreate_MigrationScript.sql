CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250614201917_InitialCreate') THEN
    CREATE TABLE "Tour" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Price" numeric(18,2) NOT NULL,
    "StartDate" date NOT NULL,
    "EndDate" date NOT NULL,
    CONSTRAINT "PK_Tour" PRIMARY KEY ("Id")
);
END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250614201917_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250614201917_InitialCreate', '9.0.6');
END IF;
END $EF$;
COMMIT;
