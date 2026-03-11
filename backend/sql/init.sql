-- run mysql -h 0.250.250.254 -P <PORT> -u <USERNAME> -p < backend/sql/init.sql
CREATE DATABASE IF NOT EXISTS midterm;

USE midterm;

CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    login VARCHAR(50) NOT NULL UNIQUE,
    pin VARCHAR(5) NOT NULL,
    balance DECIMAL(18, 2) NULL,
    is_admin BOOLEAN NOT NULL DEFAULT FALSE
);

INSERT INTO
    users (login, pin, balance, is_admin)
VALUES ('admin', '12345', NULL, TRUE)
ON DUPLICATE KEY UPDATE
    login = login;

INSERT INTO
    users (login, pin, balance, is_admin)
VALUES (
        'Adnan123',
        '12345',
        158100.00,
        FALSE
    )
ON DUPLICATE KEY UPDATE
    login = login;