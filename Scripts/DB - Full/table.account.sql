USE tasly;

CREATE TABLE `account` (
  tenant INTEGER NOT NULL,
  id INTEGER NOT NULL,
      PRIMARY KEY (tenant, id),
  name VARCHAR(255) NOT NULL,
      UNIQUE KEY (tenant, name),
  `type` SMALLINT NOT NULL
);
