using BankAccountManager.Domain.Transaction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Interface;
public interface ITransactionRepository
{
    public Task<TransactionModel?> GetById(int transactionId);
    public Task<TransactionModel?> GetLatestTransactionFromAccount(int accountId);
    public Task<List<TransactionModel>> GetFromTransactionDateByAccountId(int accountId, DateTime startTransactionDate);
    public Task<List<TransactionModel>> GetMonthlyDebitCredit(DateTime month);
    public Task<List<TransactionModel>> GetMonthlyDebitCreditByAccountId(int accountId, DateTime month);
    public Task<List<TransactionModel>> GetTransactionWithoutTypeFromTransactionDate(int accountId, DateTime startTransactionDate);

    public void AddTransaction(TransactionModel transaction);
    public void RemoveTransaction(TransactionModel transaction);

    Task SaveToDatabase();
}
