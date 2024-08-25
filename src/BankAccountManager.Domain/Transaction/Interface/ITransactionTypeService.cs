using BankAccountManager.Domain.Transaction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Interface;
public interface ITransactionTypeService
{
    Task<bool> AddType(string newType);
    Task<bool> EditType(int transactionTypeId, string newType);
    Task<List<TransactionTypeModel>> GetAll();
    Task<TransactionTypeModel?> GetById(int transactionTypeId);
}
