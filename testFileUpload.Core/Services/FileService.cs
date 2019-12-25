using System;
using System.IO;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public class FileService : IFileService
    {
        private readonly ICurrencyService _currencyService;

        public FileService(ICurrencyService currencyService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }
        public readonly string Csv = "application/vnd.ms-excel";
        public readonly string Xml = "text/xml";
        public ImportResult Import(string contentType, FileStream stream)
        {
            ImportResult result = new ImportResult()
            {
                Status = ImportResultStatus.InvalidType
            };
            Importer importer = new NullImporter();
            if (contentType == Csv)
            {
                importer = new CsvImporter(_currencyService);
            }
            else if (contentType == Xml)
            {
                importer = new XmlImporter(_currencyService);
            }

            result = importer.Validate(stream);
            //add transaction to repo here

            return result;
        }
    }
}
