using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using testFileUpload.Core.Data;
using testFileUpload.Core.Models;
using testFileUpload.Core.Services;
using testFileUpload.Models;

namespace testFileUpload.Controllers
{
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(
            ILogger<TransactionsController> logger, 
            ITransactionService transactionService, 
            IConfiguration config,
            IMapper mapper)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Route("transactions/byCurrency")]
        [HttpGet]
        public async Task<IActionResult> ByCurrency(string currency)
        {
            //validate input currency

            var transactions =await _transactionService.GetByCurrency(currency);


            if (!transactions.Any())
            {
                return NotFound();
            }
            var mappedResult = _mapper.Map<IList<Transaction>, IList<TransactionViewModel>>(transactions);
            return Ok(mappedResult);
        }

        [Route("transactions/byDateRange")]
        [HttpPost]
        public async Task<IActionResult> ByDateRange(DateTime startDate,DateTime endDate)
        {
            //validate startDate endDate
            if (startDate < endDate) return BadRequest();

            var transactions = await _transactionService.GetByDateRange(startDate,endDate);


            if (!transactions.Any())
            {
                return NotFound();
            }

            var mappedResult = _mapper.Map<IList<Transaction>, IList<TransactionViewModel>>(transactions);
            return Ok(mappedResult);
        }

        [Route("transactions/byStatus")]
        [HttpPost]
        public async Task<IActionResult> ByStatus(TransactionStatus status)
        {
            //validate status

            var transactions = await _transactionService.GetByStatus(status);


            if (!transactions.Any())
            {
                return NotFound();
            }

            var mappedResult = _mapper.Map<IList<Transaction>, IList<TransactionViewModel>>(transactions);
            return Ok(mappedResult);
        }

    }
}
