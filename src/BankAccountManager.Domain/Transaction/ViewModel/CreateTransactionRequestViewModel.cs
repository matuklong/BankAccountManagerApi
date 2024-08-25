using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.ViewModel;

public enum TransactionAmountType { TransactionAmount, AccountBalance };

public class CreateTransactionRequestViewModel
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public bool CapitalizationEvent { get; set; }
    public bool TransferenceBetweenAccounts { get; set; }
    public TransactionAmountType TransactionAmountType { get; set; }

}
