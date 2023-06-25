DROP TABLE IF EXISTS mark_list, irritation_history;

CREATE TABLE mark_list (
  id INT AUTO_INCREMENT PRIMARY KEY,
  create_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  width INT NOT NULL,
  height INT NOT NULL,
  total_anger_point INT NOT NULL DEFAULT 0,
  burst_flag BOOLEAN DEFAULT FALSE
);

CREATE TABLE irritation_history (
  id INT AUTO_INCREMENT PRIMARY KEY,
  create_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  mark_id INT NOT NULL,
  comment VARCHAR(200),
  anger_point INT NOT NULL,
  FOREIGN KEY(mark_id) REFERENCES mark_list(id)
);