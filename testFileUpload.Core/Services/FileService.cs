using System;
using System.IO;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public class FileService : IFileService
    {
        private readonly ICurrencyService _currencyService;
        private readonly string _csv = "application/vnd.ms-excel";
        private readonly string _xml = "text/xml";

        public FileService(ICurrencyService currencyService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }

        public ImportResult Import(string contentType, FileStream stream)
        {
            Importer importer = new NullImporter();
            if (contentType == _csv)
            {
                importer = new CsvImporter(_currencyService);
            }
            else if (contentType == _xml)
            {
                importer = new XmlImporter(_currencyService);
            }

            var result = importer.Validate(stream);

            return result;
        }
    }
}