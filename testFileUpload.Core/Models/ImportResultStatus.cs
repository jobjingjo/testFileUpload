using System;
using System.Collections.Generic;
using System.Text;

namespace testFileUpload.Core.Models
{
    public enum ImportResultStatus
    {
        InvalidType,
        InvalidValidation,
        SystemError,
        Ok,
        InvalidSize
    }
}
