using System;
using System.Collections.Generic;
using System.Text;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Data
{
    public class TransactionsRepository : EfCoreRepository<Transaction, AppDbContext>
    {
        public TransactionsRepository(AppDbContext context) : base(context)
        {

        }
    }
}
