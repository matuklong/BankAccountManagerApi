using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Model;
using BankAccountManager.Domain.Transaction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Service;


public class TransactionTypeService : ITransactionTypeService
{
    private readonly ITransactionTypeRepository _transactionTypeRepository;

    public TransactionTypeService(ITransactionTypeRepository transactionTypeRepository)
    {
        _transactionTypeRepository = transactionTypeRepository;
    }

    public async Task<List<TransactionTypeModel>> GetAll()
    {
        return await _transactionTypeRepository.GetAll();
    }

    public async Task<TransactionTypeModel?> GetById(int transactionTypeId)
    {
        return await _transactionTypeRepository.GetById(transactionTypeId);
    }

    public async Task<bool> AddType(string newType)
    {
        var transactionTypeModel = new TransactionTypeModel(newType);
        await _transactionTypeRepository.AddType(transactionTypeModel);
        return true;
    }

    public async Task<bool> EditType(int transactionTypeId, string newType)
    {
        var transactionTypeModel = await _transactionTypeRepository.GetById(transactionTypeId);
        if (transactionTypeModel == null)
            return false;

        transactionTypeModel.ChangeType(newType);
        await _transactionTypeRepository.EditType(transactionTypeModel);
        return true;
    }
}
