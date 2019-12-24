﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetByCurrency(string currency);
        Task<List<Transaction>> GetByDateRange(DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetByStatus(TransactionStatus status);
        Task SaveTransaction(IList<Transaction> transactions);
    }
}
