using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace testFileUpload.Core.Models
{
    public class Transaction
    {
        [Required]
        [MaxLength(50)]
        public string Id { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public TransactionStatus Status { get; set; }
    }
}
