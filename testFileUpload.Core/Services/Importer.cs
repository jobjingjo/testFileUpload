using System;
using System.IO;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public abstract class Importer
    {
        public Importer() { 
        
        }
        public virtual ImportResult Validate(FileStream stream)
        {
            throw new NotImplementedException();
        }
    }

    public class NullImporter : Importer
    {
        public override ImportResult Validate(FileStream stream)
        {
            return new ImportResult()
            {
                Status = ImportResultStatus.InvalidType
            };
        }
    }
}
