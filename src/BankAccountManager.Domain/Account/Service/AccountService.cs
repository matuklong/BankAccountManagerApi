using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Account.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Account.Service;
public class AccountService: IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountModel?> GetById(int accountId)
    {
        return await _accountRepository.GetById(accountId);
    }

    public async Task<List<AccountModel>> GetActiveAccounts()
    {
        return await _accountRepository.GetAllActive();
    }

    public async Task<AccountModel?> CreateAccount(CreateAccountViewModel createAccountViewModel)
    {
        var account = AccountModel.Create(createAccountViewModel.AccountNumber, createAccountViewModel.AccountHolder, createAccountViewModel.Description);
        return await _accountRepository.CreateAccount(account);
    }
}
