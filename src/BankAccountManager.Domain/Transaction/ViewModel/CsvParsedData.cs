using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.ViewModel;
public class CsvParsedData
{
    public DateTime? TransactionDate { get; set; }   
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
}
