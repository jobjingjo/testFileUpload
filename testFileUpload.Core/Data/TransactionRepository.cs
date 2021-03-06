﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var transactions = await _context.Transactions.Where(t => t.CurrencyCode == currency)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
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

        public async Task<bool> SaveTransaction(IList<Transaction> transactions)
        {
            await _context.Transactions.AddRangeAsync(transactions);
            var change = await _context.SaveChangesAsync();
            return change == transactions.Count;
        }
    }
}