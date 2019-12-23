using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public interface ITransactionService
    {
        Task<List<Models.Transaction>> GetByCurrency(string currency);
        Task<List<Models.Transaction>> GetByDateRange(DateTime startDate, DateTime endDate);
        Task<List<Models.Transaction>> GetByStatus(TransactionStatus status);
    }
}
