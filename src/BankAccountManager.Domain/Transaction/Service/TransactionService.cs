using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Model;
using BankAccountManager.Domain.Transaction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Service;
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<List<TransactionModel>> CreateTransaction(AccountModel account, List<CreateTransactionRequestViewModel> createTransactionRequestList)
    {
        // var account = await _accountRepository.GetActiveById(createTransactionRequestViewModel.AccountId);
        if (account == null)
            throw new Exception($"Account cannot be null");

        var resultList = new List<TransactionModel>();
        foreach (var createTransactionRequestViewModel in createTransactionRequestList)
        {

            var transactionAmount = createTransactionRequestViewModel.TransactionAmountType switch
            {
                TransactionAmountType.TransactionAmount => createTransactionRequestViewModel.Amount,
                TransactionAmountType.AccountBalance => createTransactionRequestViewModel.Amount - account.Balance,
                _ => throw new Exception($"Transaction Amount Type {createTransactionRequestViewModel.TransactionAmountType} not supported")
            };

            // Create Transaction
            var transaction = new TransactionModel(
                account,
                transactionAmount,
                createTransactionRequestViewModel.Description ?? "",
                createTransactionRequestViewModel.TransactionDate,
                DateTime.Now,
                createTransactionRequestViewModel.CapitalizationEvent,
                createTransactionRequestViewModel.TransferenceBetweenAccounts
                );

            // Update Account Balance
            account.AddTransaction(transaction);

            // Add Itens to Context
            _transactionRepository.AddTransaction(transaction);

            // Return created Transactions
            resultList.Add(transaction);
        }

        // Save to Database using transaction to ensure Data Consistency
        await _transactionRepository.SaveToDatabase();

        // check if all Ids were created
        if (resultList.Any(x => x.Id <= 0))
            throw new Exception("Transaction Ids not created");

        return resultList;
    }


    public async Task<List<TransactionModel>> GetTransactions(AccountModel account, DateTime startTransactionDate)
    {
        var transactions = await _transactionRepository.GetFromTransactionDateByAccountId(account.Id, startTransactionDate);
        if (transactions.Any())
            return transactions;

        var lastAccountTransaction = await _transactionRepository.GetLatestTransactionFromAccount(account.Id);
        if (lastAccountTransaction != null)
            return new List<TransactionModel> { lastAccountTransaction };
        else
            return new List<TransactionModel>();
    }

    public async Task DeleteTransaction(AccountModel account, int transactionId)
    {
        // var account = await _accountRepository.GetActiveById(createTransactionRequestViewModel.AccountId);
        if (account == null)
            throw new Exception($"Account cannot be null");

        var currentTransaction = await _transactionRepository.GetById(transactionId);
        if (currentTransaction == null)
            throw new Exception($"TransactionId {transactionId} not found for delete");

        account.RemoveTransaction(currentTransaction);

        _transactionRepository.RemoveTransaction(currentTransaction);

        // Save to Database using transaction to ensure Data Consistency
        await _transactionRepository.SaveToDatabase();

    }
}
