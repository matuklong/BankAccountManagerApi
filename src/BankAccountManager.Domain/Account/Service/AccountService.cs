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
        if (createAccountViewModel == null)
            throw new ArgumentNullException(nameof(createAccountViewModel));

        if (string.IsNullOrWhiteSpace(createAccountViewModel.AccountNumber))
            throw new ArgumentException("Account number is required.", nameof(createAccountViewModel.AccountNumber));

        if (string.IsNullOrWhiteSpace(createAccountViewModel.AccountHolder))
            throw new ArgumentException("Account holder is required.", nameof(createAccountViewModel.AccountHolder));

        var account = AccountModel.Create(createAccountViewModel.AccountNumber, createAccountViewModel.AccountHolder, createAccountViewModel.Description ?? "");
        return await _accountRepository.CreateAccount(account);
    }
}
