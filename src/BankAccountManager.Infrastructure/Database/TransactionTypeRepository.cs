using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Infrastructure.Database;
internal class TransactionTypeRepository : ITransactionTypeRepository
{
    private readonly BankAccountContext _bankAccountContext;

    public TransactionTypeRepository(BankAccountContext bankAccountContext)
    {
        _bankAccountContext = bankAccountContext;
    }

    public async Task<List<TransactionTypeModel>> GetAll()
    {
        return await _bankAccountContext.transactionTypes.Include(x => x.TransactionTypeString).ToListAsync();
    }

    public async Task<TransactionTypeModel?> GetById(int transactionTypeId)
    {
        return await _bankAccountContext.transactionTypes.FindAsync(transactionTypeId);
    }

    public async Task AddType(TransactionTypeModel transactionTypeModel)
    {
        await _bankAccountContext.transactionTypes.AddAsync(transactionTypeModel);
        await _bankAccountContext.SaveChangesAsync();
    }

    public async Task EditType(TransactionTypeModel transactionTypeModel)
    {
        await _bankAccountContext.SaveChangesAsync();
    }
}
