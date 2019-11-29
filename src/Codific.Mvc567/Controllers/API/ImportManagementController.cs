using System;
using System.Threading.Tasks;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Codific.Mvc567.Controllers.API
{
    [Route("api/import-management")]
#if !DEBUG
    [Authorize(Policy = Policies.AuthorizedUploadPolicy)]
#endif
    public class ImportManagementController : Controller
    {
        private IExcelImportService excelImportService;

        public ImportManagementController(IExcelImportService excelImportService)
        {
            this.excelImportService = excelImportService;
        }

        [HttpGet]
        [Route("excel-preview/{fileId}/{rows}/{columns}")]
        public IActionResult PreviewExcelFile(Guid fileId, int rows, int columns)
        {
            string filePath = @"/Users/georgi/Documents/workspace/mvc567 Application/Application/privateroot/uploads/temp/fdb561eb832f0cee1669b9e05bfb83e97.xlsx";
            var result = this.excelImportService.ExtractLinesOfExcelFile(filePath, rows, columns);

            return this.Ok(result);
        }
    }
}
