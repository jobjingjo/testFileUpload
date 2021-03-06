﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using testFileUpload.Core.Models;
using testFileUpload.Core.Types;

namespace testFileUpload.Core.Services
{
    public class CsvImporter : Importer
    {
        private readonly ICurrencyService _currencyService;
        private readonly string AMOUNT = "Amount";
        private readonly int AMOUNT_INDEX = 1;
        private readonly string CURRENCY_CODE = "CurrencyCode";
        private readonly int CURRENCY_INDEX = 2;
        private readonly int DATE_INDEX = 3;
        private readonly string ID = "Id";
        private readonly int ID_INDEX = 0;
        private readonly int MAXLENGTH = 50;
        private readonly string STATUS = "Status";
        private readonly int STATUS_INDEX = 4;
        private readonly string TRANSACTION_DATE = "TransactionDate";

        public CsvImporter(ICurrencyService currencyService)
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
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    var lines = text.Split(new[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
                    var elementIndex = 0;
                    if (lines.Length == 0)
                    {
                        result.Status = ImportResultStatus.NoData;
                        return result;
                    }

                    foreach (var line in lines)
                    {
                        //process line
                        if (!line.Contains("\","))
                        {
                            result.AddError(elementIndex, "incorrect format");
                        }
                        else
                        {
                            var args = line.Split(new[] {"\","}, StringSplitOptions.RemoveEmptyEntries);

                            if (args.Length < 5)
                            {
                                result.AddError(elementIndex, "not enough parameter");
                            }
                            else
                            {
                                for (var i = 0; i < args.Length; i++)
                                {
                                    args[i] = args[i].Replace("\"", "").Trim();
                                }

                                var id = ValidateId(result, elementIndex, args);

                                var dAmount = ValidateAmount(result, elementIndex, args);

                                var detailCurrencyCode = ValidateCurrency(result, elementIndex, args);

                                var dtTransactionDate = ValidateDate(result, elementIndex, args);

                                var tsTransactionStatus = ValidateStatus(result, elementIndex, args);
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
                            }
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
            }
            catch
            {
                result.Status = ImportResultStatus.SystemError;
            }

            return result;
        }

        private TransactionStatus ValidateStatus(ImportResult result, int elementIndex, string[] args)
        {
            var status = args[STATUS_INDEX];
            if (string.IsNullOrEmpty(status))
            {
                result.AddError(elementIndex, $"{STATUS} not found");
            }

            if (!Enum.TryParse(typeof(CsvStatus), status, out var tsTransactionStatus))
            {
                result.AddError(elementIndex, $"{CURRENCY_CODE} not found");
            }

            switch (tsTransactionStatus)
            {
                case CsvStatus.Approved:
                    return TransactionStatus.Approved;
                case CsvStatus.Failed:
                    return TransactionStatus.Rejected;
                case CsvStatus.Finished:
                    return TransactionStatus.Done;
                default:
                    return TransactionStatus.Unknow;
            }
        }

        private DateTime ValidateDate(ImportResult result, int elementIndex, string[] args)
        {
            var transactionDate = args[DATE_INDEX];
            if (string.IsNullOrEmpty(transactionDate))
            {
                result.AddError(elementIndex, $"{TRANSACTION_DATE} not found");
            }

            if (!DateTime.TryParseExact(transactionDate, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var dtTransactionDate))
            {
                result.AddError(elementIndex, $"{TRANSACTION_DATE} incorrect format");
            }

            return dtTransactionDate;
        }

        private string ValidateCurrency(ImportResult result, int elementIndex, string[] args)
        {
            var detailCurrencyCode = args[CURRENCY_INDEX];
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

        private decimal ValidateAmount(ImportResult result, int elementIndex, string[] args)
        {
            var detailAmount = args[AMOUNT_INDEX];
            if (string.IsNullOrEmpty(detailAmount))
            {
                result.AddError(elementIndex, $"{AMOUNT} not found");
            }

            if (!decimal.TryParse(detailAmount, out var dAmount))
            {
                result.AddError(elementIndex, $"{AMOUNT} incorrect format");
            }

            return dAmount;
        }

        private string ValidateId(ImportResult result, int elementIndex, string[] args)
        {
            var id = args[ID_INDEX];
            if (string.IsNullOrEmpty(id))
            {
                result.AddError(elementIndex, $"{ID} not found");
            }

            if (ID.Length > MAXLENGTH)
            {
                result.AddError(elementIndex, $"{ID} max length is {MAXLENGTH}");
            }

            return id;
        }
    }
}