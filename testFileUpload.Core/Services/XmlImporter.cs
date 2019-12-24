using System;
using System.IO;
using System.Xml;
using testFileUpload.Core.Models;

namespace testFileUpload.Core.Services
{
    public class XmlImporter :Importer
    {

        public override ImportResult Validate(FileStream stream)
        {
            var result= new ImportResult()
            {
                Status = ImportResultStatus.InvalidType
            };

            try
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    IgnoreProcessingInstructions = true,
                    ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings
                };

                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.Name.Equals("transaction") && (reader.NodeType == XmlNodeType.Element))
                        {
                            string id = reader.GetAttribute("id");
                            string TransactionDate = reader.GetAttribute("TransactionDate");
                            //string desc = reader.("desc");
                            Console.WriteLine("{0} {1}", id, TransactionDate);
                        }
                    };
                         //.Select(x => new XmlTransaction
                         //{
                         //    Id = (string)x.Attribute("Id"),
                         //    TransactionDate = (DateTime)x.Element("TransactionDate"),
                         //    //PaymentDetails = (PaymentDetails)x.Element("PaymentDetails"),
                         //    //XmlStatus = (XmlStatus)x.Element("Status")         
                         //})
                         //.ToList<XmlTransaction>();
                }
                result.Status = ImportResultStatus.Ok;
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                result.Status = ImportResultStatus.SystemError;
            }

            return result;
        }
    }
}
