using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Infrastructure.Database;
internal class AccountRepository: IAccountRepository
{
    private readonly BankAccountContext _bankAccountContext;

    public AccountRepository(BankAccountContext bankAccountContext)
    {
        _bankAccountContext = bankAccountContext;
    }
    public async Task<AccountModel?> GetById(int accountId)
    {
        return await _bankAccountContext.accountModels.FindAsync(accountId);
    }
    public async Task<AccountModel?> GetActiveById(int accountId)
    {
        var account = await _bankAccountContext.accountModels.FindAsync(accountId);
        return account?.IsActive == true ? account : null;
    }
    public async Task<List<AccountModel>> GetAllActive()
    {
        return await _bankAccountContext.accountModels.Where(x => x.IsActive).ToListAsync();
    }

    public async Task<AccountModel> CreateAccount(AccountModel account)
    {
        _bankAccountContext.accountModels.Add(account);
        await _bankAccountContext.SaveChangesAsync();
        return account;
    }
}
