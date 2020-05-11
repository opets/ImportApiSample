CREATE DATABASE IF NOT EXISTS tasly;

USE tasly;

CREATE TABLE tenant (
  id INTEGER NOT NULL AUTO_INCREMENT,
      PRIMARY KEY (id),
  name VARCHAR(255) NOT NULL,
      UNIQUE KEY (name)
);

CREATE TABLE journal_entry (
  tenant INTEGER NOT NULL,
      FOREIGN KEY (tenant) REFERENCES tenant (id),
  tx_date DATE NOT NULL,
  tx_num INTEGER NOT NULL,
      PRIMARY KEY (tenant, tx_date, tx_num),
      UNIQUE KEY (tenant, tx_num),
  currency CHAR(3) DEFAULT NULL,
  doc_number VARCHAR(32) DEFAULT NULL,
  private_note VARCHAR(4000) DEFAULT NULL
);

CREATE TABLE `account` (
  tenant INTEGER NOT NULL,
  id INTEGER NOT NULL,
      PRIMARY KEY (tenant, id),
  name VARCHAR(255) NOT NULL,
      UNIQUE KEY (tenant, name),
  `type` SMALLINT NOT NULL
);

CREATE TABLE journal_line (
  tenant INTEGER NOT NULL,
  tx_date DATE NOT NULL,
  tx_num INTEGER NOT NULL,
  line_num INTEGER NOT NULL,
      PRIMARY KEY (tenant, tx_date, tx_num, line_num),
      FOREIGN KEY (tenant, tx_date, tx_num)
          REFERENCES journal_entry(tenant, tx_date, tx_num)
          ON DELETE CASCADE ON UPDATE CASCADE,
  amount BIGINT NOT NULL,
  `account` INTEGER NOT NULL,
  description VARCHAR(4000) DEFAULT NULL
);