using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankAccountManager.Domain.Account.Model;

namespace BankAccountManager.Domain.Transaction.Model;

public class TransactionModel
{
    public int Id { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Description { get; private set; } = default!;

    /// <summary>
    /// Saves the balance before the transaction
    /// </summary>
    public decimal BalanceAtBeforeTransaction { get; private set; }

    /// <summary>
    /// Identify if the transaction is a capitalization event
    /// </summary>
    public bool CapitalizationEvent { get; private set; }

    /// <summary>
    /// Identify if the transaction is a transference between accounts
    /// </summary>
    public bool TransferenceBetweenAccounts { get; private set; }

    public int AccountId { get; private set; }
    public int? TransactionTypeId { get; private set; }
    public AccountModel Account { get; private set; }
    public TransactionTypeModel? TransactionType { get; private set; }

    private string GetDescrpitionSize(string descrption)
    {
        if (descrption.Length > 50)
            return descrption.Substring(0, 50);
        else
            return descrption;
    }

    public TransactionModel()
    {

    }

    public TransactionModel(AccountModel account, decimal amount, string description, 
        DateTime transactionDate, DateTime createdAt,
        bool capitalizationEvent, bool transferenceBetweenAccounts)
    {
        Account = account;
        AccountId = account.Id;
        Amount = amount;
        BalanceAtBeforeTransaction = account.Balance;
        Description = GetDescrpitionSize(description);
        TransactionDate = transactionDate;
        CreatedAt = createdAt;
        CapitalizationEvent = capitalizationEvent;
        TransferenceBetweenAccounts = transferenceBetweenAccounts;
    }

    public void ChangeTransaction(decimal amount, DateTime transactionDate, string description,
        bool capitalizationEvent, bool transferenceBetweenAccounts)
    {
        Amount = amount;
        Description = GetDescrpitionSize(description);
        TransactionDate = transactionDate;
        CapitalizationEvent = capitalizationEvent;
        TransferenceBetweenAccounts = transferenceBetweenAccounts;
    }

    public void ChangeTransactionType(TransactionTypeModel transactionType)
    {
        TransactionType = transactionType;
        TransactionTypeId = transactionType.Id;
    }
}
