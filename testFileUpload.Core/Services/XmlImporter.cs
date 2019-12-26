using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using testFileUpload.Core.Models;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Services
{
    public class XmlImporter : Importer
    {
        private readonly ICurrencyService _currencyService;
        private readonly string AMOUNT = "Amount";
        private readonly string CURRENCY_CODE = "CurrencyCode";
        private readonly string ID = "Id";
        private readonly int MAXLENGTH = 50;
        private readonly string PAYMENT_DETAILS = "PaymentDetails";
        private readonly string STATUS = "Status";
        private readonly string TRANSACTION_DATE = "TransactionDate";

        public XmlImporter(ICurrencyService currencyService)
        {
            _currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }

        public override ImportResult Validate(FileStream stream)
        {
            var result = new ImportResult
            {
                Status = ImportResultStatus.InvalidType
            };

            try
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    IgnoreProcessingInstructions = true,
                    ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings
                };

                using (var reader = XmlReader.Create(stream, settings))
                {
                    reader.Read();
                    if (reader.Name.Equals("Transactions") && reader.NodeType == XmlNodeType.Element)
                    {
                        var eventData = (XElement) XNode.ReadFrom(reader);
                        var elementIndex = 0;
                        foreach (var row in eventData.Elements("Transaction"))
                        {
                            var id = ValidateId(result, elementIndex, row);

                            var dtTransactionDate = ValidateTransactionDate(result, elementIndex, row);
                            var detail = ValidatePaymentDetails(result, elementIndex, row);

                            var dAmount = ValidateAmount(result, elementIndex, detail);

                            var detailCurrencyCode = ValidateCurrencyCode(result, elementIndex, detail);
                            var tsTransactionStatus = ValidateTransactionStatus(result, elementIndex, row);

                            if (!result.Errors.Any())
                            {
                                var transaction = new Transaction
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
                        else
                        {
                            result.Status = ImportResultStatus.Ok;
                        }
                    }
                    else
                    {
                        result.AddError(new ValidationError("no root element"));
                        result.Status = ImportResultStatus.InvalidValidation;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                result.Status = ImportResultStatus.SystemError;
            }

            return result;
        }

        private string ValidateCurrencyCode(ImportResult result, int elementIndex, XElement detail)
        {
            var detailCurrencyCode = (string) detail.Element(CURRENCY_CODE);
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
            var detailAmount = (string) row.Element(AMOUNT);
            if (string.IsNullOrEmpty(detailAmount))
            {
                result.AddError(elementIndex, $"{AMOUNT} not found");
            }

            if (!decimal.TryParse(detailAmount, out var dDetailAmount))
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
            var transactionDate = (string) row.Element(TRANSACTION_DATE);
            if (string.IsNullOrEmpty(transactionDate))
            {
                result.AddError(elementIndex, $"{TRANSACTION_DATE} not found");
            }

            if (!DateTime.TryParseExact(transactionDate, "yyyy-MM-dd'T'HH:mm:ss", null, DateTimeStyles.None,
                out var dtTransactionDate))
            {
                result.AddError(elementIndex, $"{TRANSACTION_DATE} incorrect format");
            }

            return dtTransactionDate;
        }

        private string ValidateId(ImportResult result, int elementIndex, XElement row)
        {
            var id = (string) row.Attribute(ID);
            if (string.IsNullOrEmpty(id))
            {
                result.AddError(elementIndex, $"{ID} not found");
            }
            else if (id.Length > MAXLENGTH)
            {
                result.AddError(elementIndex, $"{ID} max length is {MAXLENGTH}");
            }

            return id;
        }

        private TransactionStatus ValidateTransactionStatus(ImportResult result, int elementIndex, XElement row)
        {
            var status = (string) row.Element(STATUS);
            if (string.IsNullOrEmpty(status))
            {
                result.AddError(elementIndex, $"{STATUS} not found");
            }

            if (!Enum.TryParse(typeof(XmlStatus), status, out var tsTransactionStatus))
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
                default:
                    return TransactionStatus.Unknow;
            }
        }
    }
}