using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testFileUpload.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ToXml()
        {
            TransactionFile file = new TransactionFile();
            file.Add(new Transaction()
            {
                Id = "Inv00001",
                TransactionDate = DateTime.Parse("2019-01-23T13:45:10"),
                PaymentDetails = new PaymentDetails()
                {
                    Amount = 200,
                    CurrencyCode = "USD"
                },
                Status = Status.Done
            });
            file.Add(new Transaction()
            {
                Id = "Inv00002",
                TransactionDate = DateTime.Parse("2019-01-24T16:09:15"),
                PaymentDetails = new PaymentDetails()
                {
                    Amount = 1000,
                    CurrencyCode = "EUR"
                },
                Status = Status.Rejected
            });
            Assert.AreEqual("", file.ToXml());

        }

        [TestMethod]
        public void LoadCSV()
        {

        }

        [TestMethod]
        public void LoadXML()
        {

        }
    }
}
