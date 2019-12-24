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
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Transaction>> GetByCurrency(string currency)
        {
            var transactions = await _context.Transactions.Where(t => t.CurrencyCode == currency)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Where(t => t.TransactionDate>=startDate)
                .Where(t => t.TransactionDate <= endDate)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetByStatus(TransactionStatus status)
        {
            var transactions = await _context.Transactions.Where(t => t.Status == status)
               .AsNoTracking()
               .ToListAsync();
            return transactions;
        }
    }
}
