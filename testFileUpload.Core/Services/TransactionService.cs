using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testFileUpload.Core.Data;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionsRepository _transactionsRepository;

        //private readonly AppDbContext _context;

        public TransactionService(
            ITransactionsRepository transactionsRepository
            ) {
            _transactionsRepository = transactionsRepository ?? throw new ArgumentNullException(nameof(transactionsRepository));
        }

        public async Task<List<Transaction>> GetByCurrency(string currency)
        {
            return await _transactionsRepository.GetByCurrency(currency);
        }

        public async Task<List<Transaction>> GetByDateRange(DateTime startDate, DateTime endDate)
        {    
            return await _transactionsRepository.GetByDateRangeAsync(startDate, endDate);
        }

        public async Task<List<Transaction>> GetByStatus(TransactionStatus status)
        {
            return await _transactionsRepository.GetByStatus(status);
        }
    }
}
