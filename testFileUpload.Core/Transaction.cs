using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace testFileUpload.Core
{
    public class Transaction
    {
        [XmlAttribute]
        [MaxLength(50)]
        public string Id { get; set; }
        //Date Format yyyy-MM-ddThh:mm:ss e.g. 2019-0123T13:45:10 
        public DateTime TransactionDate { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public Status Status { get; set; }
    }

    public class PaymentDetails
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public enum Status
    {
        Unknown = 0,
        Approved,
        Rejected,
        Done
    }

    [XmlRoot("Transactions")]
    public class TransactionFile : List<Transaction>
    {

    }

    public static class TransactionHelper
    {
        public static string ToXml<T>(this T transaction)
        {
            using var stringWriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringWriter, transaction);
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
