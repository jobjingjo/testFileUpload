using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using testFileUpload.Core.Models;
using testFileUpload.Core.Services;

namespace testFileUpload.Core.Tests.Services
{
    [TestClass]
    public class ImporterTests
    {
        [TestMethod]
        public void XmlImporterTests()
        {
            var target = new XmlImporter(new CurrencyService());
            var stream = File.Open("input.xml", FileMode.Open);
            var result = target.Validate(stream);
            Assert.AreEqual(ImportResultStatus.Ok, result.Status);
            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreNotEqual(0, result.Transactions.Count);
        }

        [TestMethod]
        public void CsvImporterTests()
        {
            var target = new CsvImporter(new CurrencyService());
            var stream = File.Open("input.csv", FileMode.Open);
            var result = target.Validate(stream);
            Assert.AreEqual(ImportResultStatus.Ok, result.Status);
            Assert.AreEqual(0, result.Errors.Count);
            Assert.AreNotEqual(0, result.Transactions.Count);
        }
    }
}