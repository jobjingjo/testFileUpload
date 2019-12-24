using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using testFileUpload.Core.Services;

namespace testFileUpload.Core.Tests.Services
{
    [TestClass]
    public class CurrencyServiceTests
    {
        private CurrencyService _target;

        [TestInitialize]
        public void Setup() {
            _target = new CurrencyService();
        }
        
        [TestMethod]
        public void Exists_WhenInISO4217_ShouldReturnTrue()
        {
            var lines = File.ReadAllLines("ISO_4217.txt");
            foreach (var line in lines) {
                var result = _target.Exists(line);
                Assert.IsTrue(result,$"{line} not found");
            }           
        }

        [TestMethod]
        public void Exists_WhenNotInTable_ShouldReturnTrue()
        {
                var result = _target.Exists("JOB");
                Assert.IsFalse(result);
        }
    }
}
