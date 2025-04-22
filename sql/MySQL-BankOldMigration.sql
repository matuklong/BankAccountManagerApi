
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



-- --------------------------------------------------------------------------
-- Validations


select count(*) as Qtty, sum(id) as sum, 'BankAccountProduction.TransactionType' as tablename from BankAccountProduction.TransactionType union
select count(*) as Qtty, sum(ID_TIPO_MOVIMENTO) as sum, 'BankOld.TIPO_MOVIMENTO' as tablename from BankOld.TIPO_MOVIMENTO union
select count(*) as Qtty, sum(id) as sum, 'BankAccountProduction.TransactionTypeIdentificator' as tablename from BankAccountProduction.TransactionTypeIdentificator union
select count(*) as Qtty, sum(ID_IDENTIFICACAO_MOVIMENTO) as sum, 'BankOld.IDENTIFICACAO_MOVIMENTO' as tablename from BankOld.IDENTIFICACAO_MOVIMENTO
;


select count(*) as Qtty, sum(id) as sum, sum(balance) balance, 'BankAccountProduction.Account' as tablename from BankAccountProduction.Account union
select count(*) as Qtty, sum(ID_CONTA) as sum, sum(SALDO) balance, 'BankOld.CONTAS' as tablename from BankOld.CONTAS
;

select count(*) as Qtty, sum(id) as sum, sum(amount) amount, 'BankAccountProduction.Transaction' as tablename from BankAccountProduction.Transaction 
union
select count(*) as Qtty, sum(ID_MOVIMENTO) as sum, sum(ROUND(VALOR, 2)) amount, 'BankOld.MOVIMENTO' as tablename from BankOld.MOVIMENTO 
;
-- 253459678	205084,47
-- 253459678	205084.4700

select *
from (
    select year(transaction_date) y, month(transaction_date) m, count(*) as Qtty, sum(id) as sum, sum(amount) amount, 'BankAccountProduction.Transaction' as tablename from BankAccountProduction.Transaction 
    group by year(transaction_date), month(transaction_date)
    ) a
join (
    select year(DT_MOVIMENTO) y, month(DT_MOVIMENTO) m, count(*) as Qtty, sum(ID_MOVIMENTO) as sum, sum(ROUND(VALOR, 2)) amount, 'BankOld.MOVIMENTO' as tablename from BankOld.MOVIMENTO 
    group by year(DT_MOVIMENTO), month(DT_MOVIMENTO)
    ) b 
    on a.y = b.y and a.m = b.m

order by a.y, a.m
;



select m.ID_TIPO_MOVIMENTO, t.transaction_type_id, tt.transaction_type, t.*, m.*
from BankAccountProduction.Transaction t
left join BankOld.MOVIMENTO m
  on m.DT_MOVIMENTO = t.transaction_date AND m.DESCRICAO = t.description and m.VALOR = t.amount
left join TransactionType tt on t.transaction_type_id = tt.id
where (
    m.ID_TIPO_MOVIMENTO <> t.transaction_type_id
    or ( m.ID_TIPO_MOVIMENTO is null and t.transaction_type_id is not null)
    or ( m.ID_TIPO_MOVIMENTO is not null and t.transaction_type_id is null)
)
  and t.transaction_date >= '2025-02-01'
;