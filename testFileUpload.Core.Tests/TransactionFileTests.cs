using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using testFileUpload.Core.Models;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Tests
{
    [TestClass]
    public class TransactionFileTests
    {
        [TestMethod]
        public void ToXml()
        {
            var file = new TransactionFile
            {
                new XmlTransaction
                {
                    Id = "Inv00001",
                    TransactionDate = DateTime.Parse("2019-01-23T13:45:10"),
                    PaymentDetails = new PaymentDetails {Amount = 200, CurrencyCode = "USD"},
                    Status = XmlStatus.Done
                },
                new XmlTransaction
                {
                    Id = "Inv00002",
                    TransactionDate = DateTime.Parse("2019-01-24T16:09:15"),
                    PaymentDetails = new PaymentDetails {Amount = 1000, CurrencyCode = "EUR"},
                    Status = XmlStatus.Rejected
                }
            };
            Console.WriteLine(file.ToXml());
            Assert.Inconclusive("Use this to create a test file");
        }
    }
}