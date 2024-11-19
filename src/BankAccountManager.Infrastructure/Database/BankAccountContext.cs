using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.Model;
using Microsoft.EntityFrameworkCore;

namespace BankAccountManager.Infrastructure.Database;
public class BankAccountContext: DbContext
{
    public DbSet<AccountModel> accountModels {   get; set; }
    public DbSet<TransactionModel> transactionModels { get; set; }
    public DbSet<TransactionTypeModel> transactionTypes { get; set; }
    public DbSet<TransactionTypeIdentificatorModel> transactionTypesIdentificatorModels { get;set; }

    public BankAccountContext(DbContextOptions<BankAccountContext> options)
    : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine("OnConfiguring: " + optionsBuilder.Options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountModel>(t =>
        {
            t.ToTable("Account");
            t.HasKey(t => t.Id);
            
            t.Property(p => p.Id).HasColumnName("id");
            t.Property(p => p.AccountNumber).HasColumnName("account_number");
            t.Property(p => p.AccountHolder).HasColumnName("account_holder");
            t.Property(p => p.Description).HasColumnName("description");
            t.Property(p => p.Balance).HasColumnName("balance");
            t.Property(p => p.CreatedAt).HasColumnName("created_at");
            t.Property(p => p.LastTransactionDate).HasColumnName("last_transaction_date");
            t.Property(p => p.IsActive).HasColumnName("is_active");


        });

        modelBuilder.Entity<TransactionModel>(t =>
        {
            t.ToTable("Transaction");
            t.HasKey(t => t.Id);
            t.HasOne(a => a.Account);
            t.HasOne(a => a.TransactionType);



            t.Property(p => p.Id).HasColumnName("id");
            t.Property(p => p.Amount).HasColumnName("amount");
            t.Property(p => p.TransactionDate).HasColumnName("transaction_date");
            t.Property(p => p.CreatedAt).HasColumnName("created_at");
            t.Property(p => p.Description).HasColumnName("description");
            t.Property(p => p.BalanceAtBeforeTransaction).HasColumnName("balance_at_before_transaction");
            t.Property(p => p.CapitalizationEvent).HasColumnName("capitalization_event");
            t.Property(p => p.TransferenceBetweenAccounts).HasColumnName("transference_between_accounts");
            t.Property(p => p.AccountId).HasColumnName("account_id");
            t.Property(p => p.TransactionTypeId).HasColumnName("transaction_type_id");

        });

        modelBuilder.Entity<TransactionTypeModel>(t =>
        {
            t.ToTable("TransactionType");
            t.HasKey(t => t.Id);
            t.HasMany(t => t.TransactionTypeString);

            t.Property(p => p.Id).HasColumnName("id");
            t.Property(p => p.TransactionType).HasColumnName("transaction_type");
        });

        modelBuilder.Entity<TransactionTypeIdentificatorModel>(t =>
        {
            t.ToTable("TransactionTypeIdentificator");
            t.HasKey(t => t.Id);

            t.Property(p => p.Id).HasColumnName("id");
            t.Property(p => p.Description).HasColumnName("description");
            t.Property(p => p.ExpectedAmount).HasColumnName("expected_amount");
            t.Property(p => p.TransactionTypeId).HasColumnName("transaction_type_id");
        });
    }
}
