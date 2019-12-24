using System.Collections.Generic;

namespace testFileUpload.Core.Models
{
    public class ImportResult { 
        public ImportResultStatus Status { get; set; }

        public IList<ValidationError> _errors = null;
        public IList<ValidationError> Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new List<ValidationError>();
                }
                return _errors;
            }
            set
            {
                _errors = value;
            }
        }

        public IList<Transaction> _transactions = null;
        public IList<Transaction> Transactions
        {
            get
            {
                if (_transactions == null)
                {
                    _transactions = new List<Transaction>();
                }
                return _transactions;
            }
            set
            {
                _transactions = value;
            }
        }
        public void AddError(int index, string error)
        {
            AddError(new ValidationError($"element index {index} :{error}"));
        }
        public void AddError(string error)
        {
            AddError(new ValidationError(error));
        }

        public void AddError(ValidationError error) {
            Errors.Add(error);
        }
    }
}
