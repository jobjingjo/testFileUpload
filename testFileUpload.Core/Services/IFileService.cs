using System.IO;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public interface IFileService
    {
        ImportResult Import(string contentType, FileStream stream);
    }
}