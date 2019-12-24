using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Data
{
    public class TransactionsRepository : EfCoreRepository<Transaction, AppDbContext>, ITransactionsRepository
    {
        private readonly AppDbContext _context;

        public TransactionsRepository(AppDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Transaction>> GetByCurrency(string currency)
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(new Transaction()
            {
                Id = "inv-xxx-xx",
                Amount = 1,
                CurrencyCode = currency,
                Status = TransactionStatus.Done,
                TransactionDate = DateTime.Today
            });
            return await Task.FromResult(transactions);

            //var transactions = await _context.Transactions.Where(t => t.CurrencyCode == currency)
            //    .AsNoTracking()
            //    .ToListAsync();
            //return transactions;
            //throw new NotImplementedException();
        }

        public async Task<List<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = await _context.Transactions
                .Where(t => t.TransactionDate >= startDate)
                .Where(t => t.TransactionDate <= endDate)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetByStatus(TransactionStatus status)
        {
            var transactions = await _context.Transactions
                .Where(t => t.Status == status)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }
    }
}
