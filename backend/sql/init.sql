-- run mysql -h 0.250.250.254 -P <PORT> -u <USERNAME> -p < backend/sql/init.sql
CREATE DATABASE IF NOT EXISTS midterm;

USE midterm;

CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    login VARCHAR(50) NOT NULL UNIQUE,
    pin VARCHAR(5) NOT NULL,
    holders_name VARCHAR(100) NULL,
    balance DECIMAL(18, 2) NULL,
    is_admin BOOLEAN NOT NULL DEFAULT FALSE,
    status VARCHAR(20) NOT NULL DEFAULT 'Active'
);

INSERT INTO
    users (
        login,
        pin,
        holders_name,
        balance,
        is_admin,
        status
    )
VALUES (
        'admin',
        '12345',
        NULL,
        NULL,
        TRUE,
        'Active'
    )
ON DUPLICATE KEY UPDATE
    login = login;

INSERT INTO
    users (
        login,
        pin,
        holders_name,
        balance,
        is_admin,
        status
    )
VALUES (
        'Adnan123',
        '12345',
        'Adnan',
        158100.00,
        FALSE,
        'Active'
    )
ON DUPLICATE KEY UPDATE
    login = login;