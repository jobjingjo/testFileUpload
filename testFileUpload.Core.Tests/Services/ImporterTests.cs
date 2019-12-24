using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            var target = new XmlImporter();
            FileStream stream = File.Open("input.xml",FileMode.Open);
            var result = target.Validate(stream);
            Assert.AreEqual(ImportResultStatus.Ok, result.Status);
        }

        [TestMethod]
        public void CsvImporterTests()
        {
            var target = new CsvImporter();
            FileStream stream = File.Open("input.csv", FileMode.Open);
            var result = target.Validate(stream);
            Assert.AreEqual(ImportResultStatus.Ok, result.Status);
        }
    }
}
