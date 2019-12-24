using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Models
{
    public class CsvTransaction
    {
        [Required]
        [MaxLength(50)]
        public string Id { get; set; }

        //Date Format yyyy-MM-ddThh:mm:ss e.g. 2019-0123T13:45:10 
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public CsvStatus Status { get; set; }
    }
}
