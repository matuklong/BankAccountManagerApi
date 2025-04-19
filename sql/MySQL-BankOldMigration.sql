
-- delete from BankAccountProduction.TransactionTypeIdentificator;
-- delete from BankAccountProduction.Account;
-- delete from BankAccountProduction.Transaction;
-- delete from BankAccountProduction.TransactionType;

-- TransactionType & TransactionTypeIdentificator


select * from BankAccountProduction.TransactionType;
select * from BankOld.TIPO_MOVIMENTO;

select * from BankAccountProduction.TransactionTypeIdentificator;
select * from BankOld.IDENTIFICACAO_MOVIMENTO;

-- insert into BankAccountProduction.TransactionType (id, transaction_type) select * from BankOld.TIPO_MOVIMENTO;
-- insert into BankAccountProduction.TransactionTypeIdentificator (id, transaction_type_id, description, expected_amount) select * from BankOld.IDENTIFICACAO_MOVIMENTO;

-- 

-- Account


select * from BankAccountProduction.Account;
select * from BankOld.CONTAS;


-- insert into BankAccountProduction.Account (id, account_holder, account_number, balance, description, last_transaction_date, is_active, created_at)
select ID_CONTA, BANCO, CONCAT(AGENCIA, '/', CONTA) as CONTA, SALDO, TIPO_CONTA, ULT_PAGTO_ARQUIVO, ATIVO, CURDATE() created_at from BankOld.CONTAS;


-- Transaction

select * from BankAccountProduction.Transaction;
select * from BankOld.MOVIMENTO;

-- select * from information_schema.COLUMNS where TABLE_NAME = 'Transaction';

-- insert into BankAccountProduction.Transaction
-- (id, account_id, transaction_type_id, 
-- amount, created_at, transaction_date, description, 
-- capitalization_event, balance_at_before_transaction, transference_between_accounts)
select ID_MOVIMENTO, ID_CONTA, ID_TIPO_MOVIMENTO, 
  ROUND(VALOR, 2) VALOR, DT_INC, DT_MOVIMENTO, DESCRICAO,
  CAPITALIZACAO, IFNULL(SALDO_NO_MOVIMENTO, 0) SALDO_NO_MOVIMENTO, TRANSFERENCIA_CONTA
from BankOld.MOVIMENTO;

