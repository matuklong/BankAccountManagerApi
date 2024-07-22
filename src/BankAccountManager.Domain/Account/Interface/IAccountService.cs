using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Account.ViewModel;
using BankAccountManager.Domain.Transaction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Account.Interface;
public interface IAccountService
{
    public Task<AccountModel?> GetById(int accountId);
    public Task<List<AccountModel>> GetActiveAccounts();
    public Task<AccountModel?> CreateAccount(CreateAccountViewModel createAccountViewModel);
}
