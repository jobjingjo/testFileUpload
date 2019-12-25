using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace testFileUpload.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly List<string> _currencySymbols = new List<string>();
        public CurrencyService() {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "testFileUpload.Core.ISO_4217.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _currencySymbols.Add(line);
                }
            }
        }
        public bool Exists(string detailCurrencyCode)
        {
            return _currencySymbols.Contains(detailCurrencyCode);
        }
    }
}