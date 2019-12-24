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

namespace testFileUpload.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileService _fileService;
        private readonly ITransactionService _transactionService;
        private readonly long _fileSizeLimit;


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

            //// To save physical files to a path provided by configuration:
            //_targetFilePath = config.GetValue<string>("StoredFilesPath");

            // To save physical files to the temporary files folder, use:
            //_targetFilePath = Path.GetTempPath();
        }
        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        if (stream.Length > _fileSizeLimit)
                        {
                            return BadRequest("File is too big");
                        }

                        var importResult = await _fileService.Import(formFile.ContentType, stream);
                        if (importResult.Status == ImportResultStatus.InvalidType)
                        {
                            return BadRequest("Unknown format");
                        }
                        else if (importResult.Status == ImportResultStatus.InvalidValidation)
                        {
                            return BadRequest();
                        }
                        else if (importResult.Status == ImportResultStatus.SystemError)
                        {
                            return Problem();
                        }

                        var success = await _transactionService.SaveTransaction(importResult.Transactions);
                        if (!success) return Problem();
                        return Ok();
                    }

                }
            }
            return BadRequest();
        }
    }
}
