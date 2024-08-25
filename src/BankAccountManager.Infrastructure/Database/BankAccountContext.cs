using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountModel>(t =>
        {
            t.ToTable("account");
            t.HasKey(t => t.Id);
        });

        modelBuilder.Entity<TransactionModel>(t =>
        {
            t.ToTable("transaction");
            t.HasKey(t => t.Id);
        });

        modelBuilder.Entity<TransactionTypeModel>(t =>
        {
            t.ToTable("transaction_type");
            t.HasKey(t => t.Id);
        });

        modelBuilder.Entity<TransactionTypeIdentificatorModel>(t =>
        {
            t.ToTable("transaction_type_identificator");
            t.HasKey(t => t.Id);
        });
    }
}
