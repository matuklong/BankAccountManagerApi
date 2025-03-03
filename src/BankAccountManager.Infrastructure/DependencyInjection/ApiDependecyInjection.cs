using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Service;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Service;
using BankAccountManager.Infrastructure.Csv;
using BankAccountManager.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankAccountManager.Infrastructure.DependencyInjection;
public static class ApiDependecyInjection
{
    public static void AddApiDependecyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddServices(services, configuration);
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        // From .Net Secrets. See readme for more info
        var connectionString = configuration.GetConnectionString("MySQL");
        services.AddDbContext<BankAccountContext>(options =>
        {
            // options.UseSqlServer("Server=localhost;Database=BankAccountManager;Trusted_Connection=True;");
            // options.UseFileContextDatabase();
            options.UseMySql(connectionString, ServerVersion.Create(Version.Parse("8.4.3"), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql));
        });

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<HealthCheckRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
    }

    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionTypeService, TransactionTypeService>();

        services.AddScoped<IFileProcessorService, CsvService>();
    }
}
