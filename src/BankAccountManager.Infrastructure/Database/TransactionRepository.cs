using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Infrastructure.Database;
internal class TransactionRepository : ITransactionRepository
{
    private readonly BankAccountContext _bankAccountContext;

    public TransactionRepository(BankAccountContext bankAccountContext)
    {
        _bankAccountContext = bankAccountContext;
    }

    public async Task<TransactionModel?> GetById(int transactionId)
    {
        return await _bankAccountContext.transactionModels.FindAsync(transactionId);
    }

    public async Task<TransactionModel?> GetLatestTransactionFromAccount(int accountId)
    {
        return await 
            _bankAccountContext.transactionModels
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.TransactionDate).ThenByDescending(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<TransactionModel>> GetFromTransactionDateByAccountId(int accountId, DateTime startTransactionDate)
    {
        return await
            _bankAccountContext.transactionModels
            .Include(x => x.TransactionType)
            .Where(x => x.AccountId == accountId && x.TransactionDate >= startTransactionDate.Date)
            .OrderBy(x => x.TransactionDate).ThenBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<TransactionModel>> GetMonthlyDebitCredit(DateTime month)
    {
        // Get First day of the month
        var baseDate = new DateTime(month.Year, month.Month, 1);

        return await
            _bankAccountContext.transactionModels
            .Include(x => x.TransactionType)
            .Where(x => x.TransactionDate >= baseDate && !x.TransferenceBetweenAccounts)
            .OrderBy(x => x.TransactionDate).ThenBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<TransactionModel>> GetMonthlyDebitCreditByAccountId(int accountId, DateTime month)
    {
        // Get First day of the month
        var baseDate = new DateTime(month.Year, month.Month, 1);

        return await
            _bankAccountContext.transactionModels
            .Include(x => x.TransactionType)
            .Where(x => x.AccountId == accountId && x.TransactionDate >= baseDate && !x.TransferenceBetweenAccounts)
            .OrderBy(x => x.TransactionDate).ThenBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<List<TransactionModel>> GetTransactionWithoutTypeFromTransactionDate(int accountId, DateTime startTransactionDate)
    {
        return await
            _bankAccountContext.transactionModels
            .Include(x => x.TransactionType)
            .Where(x => x.AccountId == accountId 
                && x.TransactionDate >= startTransactionDate.Date
                && x.TransactionType == null)

            .OrderBy(x => x.TransactionDate).ThenBy(x => x.Id)
            .ToListAsync();
    }

    public void AddTransaction(TransactionModel transaction)
    {
        _bankAccountContext.transactionModels.Add(transaction);
    }

    public void RemoveTransaction(TransactionModel transaction)
    {
        _bankAccountContext.transactionModels.Remove(transaction);
    }

    public async Task SaveToDatabase()
    {
        var currentTransaction = await _bankAccountContext.Database.BeginTransactionAsync();
        try
        {
            await _bankAccountContext.SaveChangesAsync();

            await _bankAccountContext.Database.CommitTransactionAsync();
        }
        catch
        {
            await _bankAccountContext.Database.RollbackTransactionAsync();

            throw;
        }
    }
}
