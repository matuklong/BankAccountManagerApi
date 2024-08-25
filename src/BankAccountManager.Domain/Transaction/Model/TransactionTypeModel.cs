
namespace BankAccountManager.Domain.Transaction.Model;

public class TransactionTypeModel
{
    public int Id { get; private set; }
    public string TransactionType { get; private set; } = default!;
    public List<TransactionTypeIdentificatorModel> TransactionTypeString { get; private set; } = new List<TransactionTypeIdentificatorModel>();

    public TransactionTypeModel()
    {
    }

    public TransactionTypeModel(string transactionType)
    {
        ChangeType(transactionType);
    }

    internal void ChangeType(string newTransactionType)
    {
        TransactionType = newTransactionType;
    }
}