﻿using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.Model;
using BankAccountManager.Domain.Transaction.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.Interface;
public interface ITransactionService
{
    public Task<List<TransactionModel>> CreateTransaction(AccountModel account, List<CreateTransactionRequestViewModel> createTransactionRequestList);

    public Task<List<TransactionModel>> GetTransactions(AccountModel account, DateTime startTransactionDate);
    public Task DeleteTransaction(AccountModel account, int transactionId);
}