using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankAccountManager.Infrastructure.Database;


public class HealthCheckRepository
{
    private readonly BankAccountContext _bankAccountContext;

    public HealthCheckRepository(BankAccountContext bankAccountContext)
    {
        _bankAccountContext = bankAccountContext;
    }

    public async Task<bool> CheckConnectivity(int timeoutSeconds, CancellationToken cancellationToken)
    {
        try
        {
            _bankAccountContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(timeoutSeconds));
            await _bankAccountContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
