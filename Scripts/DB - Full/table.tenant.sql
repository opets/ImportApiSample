USE tasly;

CREATE TABLE tenant (
  id INTEGER NOT NULL AUTO_INCREMENT,
      PRIMARY KEY (id),
  name VARCHAR(255) NOT NULL,
      UNIQUE KEY (name)
);