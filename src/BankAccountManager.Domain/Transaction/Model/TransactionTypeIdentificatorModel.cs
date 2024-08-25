namespace BankAccountManager.Domain.Transaction.Model;

public class TransactionTypeIdentificatorModel
{
    public int Id { get; private set; }
    public string? Description { get; private set; }
    public decimal? ExpectedAmount { get; private set; }
    public int TransactionTypeId { get; private set; }
    public TransactionTypeModel TransactionType { get; private set; } = default!;
}