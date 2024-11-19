use mysql;

CREATE DATABASE BankAccount
	CHARACTER SET utf8mb4
	COLLATE utf8mb4_0900_ai_ci;


-- DROP USER 'BankAccountUser'@'%';
CREATE USER 'BankAccountUser'@'%' IDENTIFIED  WITH mysql_native_password BY 'MySQLPassword';



GRANT SELECT, INSERT, UPDATE, DELETE ON `BankAccount`.* TO 'BankAccountUser'@'%';


use BankAccount;


CREATE TABLE if not exists Account (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_number VARCHAR(50) NOT NULL,
    account_holder VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    balance DECIMAL(18, 2) NOT NULL,
    created_at DATETIME NOT NULL,
    last_transaction_date DATETIME NULL,
    is_active BOOLEAN NOT NULL,
    INDEX idx_is_active (is_active)
);

CREATE TABLE IF NOT EXISTS TransactionType (
    id INT AUTO_INCREMENT PRIMARY KEY,
    transaction_type VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS TransactionTypeIdentificator (
    id INT AUTO_INCREMENT PRIMARY KEY,
    description TEXT NULL,
    expected_amount DECIMAL(18, 2) NULL,
    transaction_type_id INT NOT NULL,
    FOREIGN KEY (transaction_type_id) REFERENCES TransactionType(id)
);

CREATE TABLE IF NOT EXISTS Transaction (
    id INT AUTO_INCREMENT PRIMARY KEY,
    amount DECIMAL(18, 2) NOT NULL,
    transaction_date DATETIME NOT NULL,
    created_at DATETIME NOT NULL,
    description TEXT NOT NULL,
    balance_at_before_transaction DECIMAL(18, 2) NOT NULL,
    capitalization_event BOOLEAN NOT NULL,
    transference_between_accounts BOOLEAN NOT NULL,
    account_id INT NOT NULL,
    transaction_type_id INT NULL,
    FOREIGN KEY (account_id) REFERENCES Account(id),
    FOREIGN KEY (transaction_type_id) REFERENCES TransactionType(id)
);



select * from Account;
select @@version;
SELECT user, host, plugin from mysql.user WHERE plugin='mysql_native_password';