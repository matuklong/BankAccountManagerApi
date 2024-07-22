using BankAccountManager.Domain.Transaction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankAccountManager.Domain.Account.Model;
public class AccountModel
{
    public int Id { get; private set; }
    public string AccountNumber { get; private set; } = default!;
    public string AccountHolder { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastTransactionDate { get; private set; }
    public bool IsActive { get; private set; }

    public static AccountModel Create(string? accountNumber, string? accountHolder, string? description)
    {
        var account = new AccountModel
        {
            AccountNumber = accountNumber,
            AccountHolder = accountHolder,
            Description = description,
            CreatedAt = DateTime.Now,
            Balance = 0,
            IsActive = true,
        };

        return account;
    }

    public void AddTransaction(TransactionModel transaction)
    {
        Balance += transaction.Amount;

        if (LastTransactionDate == null || transaction.TransactionDate > LastTransactionDate)
            LastTransactionDate = transaction.TransactionDate;
    }

    public void RemoveTransaction(TransactionModel transaction)
    {
        Balance -= transaction.Amount;
    }
}
