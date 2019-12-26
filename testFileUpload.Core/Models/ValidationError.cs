using System;

namespace testFileUpload.Core.Models
{
    public class ValidationError
    {
        private readonly string _message;

        public ValidationError(string message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override string ToString()
        {
            return _message;
        }
    }
}