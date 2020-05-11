USE tasly;

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
  account INTEGER NOT NULL,
  description VARCHAR(4000) DEFAULT NULL
);