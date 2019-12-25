using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using testFileUpload.Core.Models;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Services
{
    public class XmlImporter :Importer
    {
        private readonly string ID = "Id";
        private readonly string TRANSACTION_DATE = "TransactionDate";
        private readonly string PAYMENT_DETAILS = "PaymentDetails";
        private readonly string AMOUNT = "Amount";
        private readonly string CURRENCY_CODE = "CurrencyCode";
        private readonly string STATUS = "Status";
        private readonly int MAXLENGTH = 50;
        private readonly ICurrencyService _currencyService;

        public XmlImporter()
        {
            _currencyService = new CurrencyService();
        }
        public XmlImporter(ICurrencyService currencyService) {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }


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
                    reader.Read();
                    if (reader.Name.Equals("Transactions") && (reader.NodeType == XmlNodeType.Element))
                    {
                        XElement eventData = (XElement)XNode.ReadFrom(reader);
                        int elementIndex = 0;
                        foreach (XElement row in eventData.Elements("Transaction"))
                        {
                            string id = ValidateId(result, elementIndex, row);

                            DateTime dtTransactionDate = ValidateTransactionDate(result, elementIndex, row);
                            XElement detail = ValidatePaymentDetails(result, elementIndex, row);

                            decimal dAmount = ValidateAmount(result, elementIndex, detail);

                            string detailCurrencyCode = ValidateCurrenyCode(result, elementIndex, detail);
                            TransactionStatus tsTransactionStatus = ValidateTransactionStauts(result, elementIndex, row);

                            if (!result.Errors.Any())
                            {
                                var transaction = new Transaction()
                                {
                                    Id = id,
                                    Amount = dAmount,
                                    TransactionDate = dtTransactionDate,
                                    CurrencyCode = detailCurrencyCode,
                                    Status = tsTransactionStatus
                                };
                                result.Transactions.Add(transaction);
                            }
                            elementIndex++;
                        }
                        if (result.Errors.Any())
                        {
                            result.Status = ImportResultStatus.InvalidValidation;
                            result.Transactions.Clear();
                        }
                        else {
                            result.Status = ImportResultStatus.Ok;
                        }
                    }
                    else {
                        result.AddError(new ValidationError("no root element"));
                        result.Status = ImportResultStatus.InvalidValidation;
                    }
                }
             
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                result.Status = ImportResultStatus.SystemError;
            }

            return result;
        }

        private string ValidateCurrenyCode(ImportResult result, int elementIndex, XElement detail)
        {
            var detailCurrencyCode = (string)detail.Element(CURRENCY_CODE);
            if (string.IsNullOrEmpty(detailCurrencyCode))
            {
                result.AddError(elementIndex, $"{CURRENCY_CODE} not found");
            }
            //validate
            if (!_currencyService.Exists(detailCurrencyCode))
            {
                result.AddError(elementIndex, $"{CURRENCY_CODE} incorrect format");
            }

            return detailCurrencyCode;
        }

        private decimal ValidateAmount(ImportResult result, int elementIndex, XElement row)
        {
            var detailAmount = (string)row.Element(AMOUNT);
            if (string.IsNullOrEmpty(detailAmount))
            {
                result.AddError(elementIndex, $"{AMOUNT} not found");
            }
            if (!Decimal.TryParse(detailAmount, out decimal dDetailAmount))
            {
                result.AddError(elementIndex, $"{AMOUNT} incorrect format");
            }
            return dDetailAmount;
        }
        private XElement ValidatePaymentDetails(ImportResult result, int elementIndex, XElement row)
        {
            var detail = row.Element(PAYMENT_DETAILS);
            if (detail == null)
            {
                result.AddError(elementIndex, $"{PAYMENT_DETAILS} not found");
            }

            return detail;
        }

        private DateTime ValidateTransactionDate(ImportResult result, int elementIndex, XElement row)
        {
            var transactionDate = (string)row.Element(TRANSACTION_DATE);
            if (string.IsNullOrEmpty(transactionDate))
            {
                result.AddError(elementIndex, $"{TRANSACTION_DATE} not found");
            }
            if (!DateTime.TryParseExact(transactionDate, "yyyy-MM-dd'T'HH:mm:ss", null, DateTimeStyles.None, out DateTime dtTransactionDate))
            {
                result.AddError(elementIndex, $"{TRANSACTION_DATE} incorrect format");
            }

            return dtTransactionDate;
        }

        private string ValidateId(ImportResult result, int elementIndex, XElement row)
        {
            var id = (string)row.Attribute(ID);
            if (string.IsNullOrEmpty(id))
            {
                result.AddError(elementIndex, $"{ID} not found");
            }
            if (id.Length > MAXLENGTH)
            {
                result.AddError(elementIndex, $"{ID} max length is {MAXLENGTH}");
            }

            return id;
        }

        private TransactionStatus ValidateTransactionStauts(ImportResult result, int elementIndex, XElement row)
        {
            var status = (string)row.Element(STATUS);
            if (string.IsNullOrEmpty(status))
            {
                result.AddError(elementIndex, $"{STATUS} not found");
            }
            if (!Enum.TryParse(typeof(XmlStatus), status, out object tsTransactionStatus))
            {
                result.AddError(elementIndex, $"{STATUS} not found");
            }

            switch (tsTransactionStatus)
            {
                case XmlStatus.Approved:
                    return TransactionStatus.Approved;
                case XmlStatus.Rejected:
                    return TransactionStatus.Rejected;
                case XmlStatus.Done:
                    return TransactionStatus.Done;
            }
            return TransactionStatus.Unknow;
        }
    }
}
