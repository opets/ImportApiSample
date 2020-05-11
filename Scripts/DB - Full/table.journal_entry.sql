USE tasly;

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
