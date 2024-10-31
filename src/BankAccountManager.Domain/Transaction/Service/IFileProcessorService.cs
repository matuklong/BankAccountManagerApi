using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Service;
public interface IFileProcessorService
{
    Task<List<FileProcessorResponseDto>> ProcessCsvAsync(Stream stream, AccountModel account, CultureInfo cultureInfo, string delimiter, bool hasHeaderRecord);
}
