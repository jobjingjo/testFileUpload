using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using testFileUpload.Core.Models;
using testFileUpload.Core.Services;

namespace testFileUpload.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly long _fileSizeLimit;
        private readonly ILogger<FileController> _logger;
        private readonly ITransactionService _transactionService;

        public FileController(
            ILogger<FileController> logger,
            IConfiguration config,
            IFileService fileService,
            ITransactionService transactionService)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            var formFile = files.FirstOrDefault();

            if (formFile == null || formFile.Length <= 0)
            {
                return BadRequest();
            }

            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
                if (formFile.Length > _fileSizeLimit)
                {
                    return BadRequest("File is too big");
                }

                stream.Seek(0, SeekOrigin.Begin);
                var importResult = _fileService.Import(formFile.ContentType, stream);
                switch (importResult.Status)
                {
                    case ImportResultStatus.InvalidType:
                        return BadRequest("Unknown format");

                    case ImportResultStatus.InvalidSize:
                        return BadRequest("Invalid size");

                    case ImportResultStatus.InvalidValidation:
                    {
                        var jsonMessage = JsonConvert.SerializeObject(importResult.Errors);
                        _logger.LogInformation(jsonMessage);
                        return BadRequest(jsonMessage);
                    }

                    case ImportResultStatus.SystemError:
                        return Problem();

                    case ImportResultStatus.NoData:
                        return BadRequest();

                    case ImportResultStatus.Ok:
                        var success = await _transactionService.SaveTransaction(importResult.Transactions);
                        if (!success)
                        {
                            return Problem();
                        }
                        return Ok();

                    default:
                        return Problem();
                }
            }
        }
    }
}