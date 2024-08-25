﻿using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Service;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Service;
using BankAccountManager.Infrastructure.Database;
using kDg.FileBaseContext.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        services.AddDbContext<BankAccountContext>(options =>
        {
            // options.UseSqlServer("Server=localhost;Database=BankAccountManager;Trusted_Connection=True;");
            // options.UseFileContextDatabase();
            options.UseFileBaseContextDatabase(location: "C:\\Users\\bruno\\Documents\\dev\\pessoal\\BankAccountManagerApi\\database_dev_files");
        });

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    }

    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
    }
}