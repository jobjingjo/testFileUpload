using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Data
{
    public interface ITransactionsRepository
    {
        Task<List<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetByStatus(TransactionStatus status);
        Task<List<Transaction>> GetByCurrency(string currency);
    }
}
