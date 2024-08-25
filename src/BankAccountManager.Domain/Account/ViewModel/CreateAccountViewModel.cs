using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Account.ViewModel;
public class CreateAccountViewModel
{
    public string? AccountNumber { get; set; }
    public string? AccountHolder { get; set; }
    public string? Description { get; set; }
}
