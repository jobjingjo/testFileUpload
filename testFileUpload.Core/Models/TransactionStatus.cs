using System.ComponentModel.DataAnnotations;

namespace testFileUpload.Core.Models
{
    public enum TransactionStatus
    {
        Unknow = 0,
        [Display(Name = "A")] Approved = 1,
        [Display(Name = "R")] Rejected = 2,
        [Display(Name = "D")] Done = 3
    }
}