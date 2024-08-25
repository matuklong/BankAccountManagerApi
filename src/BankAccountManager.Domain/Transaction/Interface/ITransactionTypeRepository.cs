using BankAccountManager.Domain.Transaction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Interface;
public interface ITransactionTypeRepository
{
    Task AddType(TransactionTypeModel transactionTypeModel);
    Task EditType(TransactionTypeModel transactionTypeModel);
    Task<List<TransactionTypeModel>> GetAll();
    Task<TransactionTypeModel?> GetById(int transactionTypeId);
}
