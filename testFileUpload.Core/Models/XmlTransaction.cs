using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Models
{
    [Serializable]
    [XmlRoot("Transaction")]
    public class XmlTransaction
    {
        [XmlAttribute]
        [MaxLength(50)]
        public string Id { get; set; }
        //Date Format yyyy-MM-ddThh:mm:ss e.g. 2019-0123T13:45:10 
        public DateTime TransactionDate { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public XmlStatus Status { get; set; }
    }

    public class PaymentDetails
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    [XmlRoot("Transactions")]
    public class TransactionFile : List<XmlTransaction>
    {

    }

    public static class TransactionHelper
    {
        public static string ToXml<T>(this T transaction)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using var stringWriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringWriter, transaction, ns);
            return stringWriter.ToString();
        }

        public static T FromXml<T>(this string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
