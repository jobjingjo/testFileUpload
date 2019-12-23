using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public class FileService : IFileService
    {
        public readonly string CSV = "text/csv";
        public readonly string XML = "application/xml";
        public Task<ImportResult> Import(string contentType, FileStream stream)
        {
            ImportResult result = new ImportResult()
            {
                Status = ImportResultStatus.InvalidType
            };
            Importer importer = new NullImporter();
            if (contentType == CSV)
            {
                importer = new CsvImporter();
            }
            else if (contentType == XML)
            {
                importer = new XmlImporter();
            }

            //result = importer.Validate(stream);
            return Task.FromResult(result);
        }
    }
}
