using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public interface IFileService
    {
        Task<ImportResult> Import(string contentType, FileStream stream);
    }
}
