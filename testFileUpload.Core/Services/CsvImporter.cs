using System;
using System.IO;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public class CsvImporter:Importer
    {
        /*
         * StreamReader reader = new StreamReader( stream );
string text = reader.ReadToEnd();
         */
        public override ImportResult Validate(FileStream stream)
        {
            var result = new ImportResult()
            {
                Status = ImportResultStatus.InvalidType
            };
            try
            {
                using (StreamReader reader = new StreamReader(stream)) {                    
                    string text = reader.ReadToEnd();
                    var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines) { 
                    //process line
                    }
                }
                result.Status = ImportResultStatus.Ok;
            }
            catch {
                result.Status = ImportResultStatus.SystemError;
            }

            return result;
        }
    }
}
