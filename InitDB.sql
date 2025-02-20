CREATE TABLE "Messages" (
    "Id" SERIAL PRIMARY KEY,
    "OrderNumber" INT NOT NULL,
    "Text" VARCHAR(128) NOT NULL,
    "SentDateTime" TIMESTAMP NOT NULL
);
