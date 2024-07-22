using BankAccountManager.Domain.Account.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Account.Interface;
public interface IAccountRepository
{
    public Task<AccountModel?> GetById(int accountId);
    public Task<AccountModel?> GetActiveById(int accountId);
    public Task<List<AccountModel>> GetAllActive();
    public Task<AccountModel> CreateAccount(AccountModel account);
}
