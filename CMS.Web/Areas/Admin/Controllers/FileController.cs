using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CMS.Web.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using CMS.Web.Filters;

namespace CMS.Web.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _targetFilePath;

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        public FileController(ILogger<FileController> logger, IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;

            _targetFilePath = configuration.GetValue<string>("StoredFilesPath");
        }

        [HttpPost("{**url}")]
        [DisableFormValueModelBinding]
        // [ValidateAntiForgeryToken]
        // [RequestSizeLimit(MaxFileSize)]
        // [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        // public async Task<IActionResult> UploadLargeFile(string url)
        public async Task<IActionResult> UploadLargeFile(string url)
        {
            // Folder
            var saveToPath = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath);
            if (!Directory.Exists(saveToPath))
            {
                Directory.CreateDirectory(saveToPath);
            }

            url ??= "";

            saveToPath = Path.Combine(saveToPath, url);

            // Create not exists folders
            if (!Directory.Exists(saveToPath))
            {
                Directory.CreateDirectory(saveToPath);
            }
            
            var request = HttpContext.Request;

            // validation of Content-Type
            // 1. first, it must be a form-data request
            // 2. a boundary should be found in the Content-Type
            if (!request.HasFormContentType ||
                !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
                string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
            {
                return new UnsupportedMediaTypeResult();
            }

            var reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
            var section = await reader.ReadNextSectionAsync();

            // This sample try to get the first file from request and save it
            // Make changes according to your needs in actual use
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out var contentDisposition);

                if (hasContentDispositionHeader && contentDisposition.DispositionType.Equals("form-data") &&
                    !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                {
                    var fileName = Path.GetFileName(contentDisposition.FileName.Value);

                    // Check if file exists - if yes - generate new name
                    if (fileName == null || string.IsNullOrEmpty(fileName) || 
                        fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0 || 
                        System.IO.File.Exists(Path.Combine(saveToPath, fileName)))
                    {
                        fileName = Path.GetRandomFileName();
                    }
                    

                    await using (var targetStream = System.IO.File.Create(Path.Combine(saveToPath, fileName)))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }
                    
                    if (!Directory.Exists(saveToPath))
                    {
                        Directory.CreateDirectory(saveToPath);
                    }

                    var imageProcess = new Thread(ImageHelpers.ResizeImg);
                    imageProcess.Start((saveToPath, fileName));
                }

                section = await reader.ReadNextSectionAsync();
            }

            // If the code runs to this location, it means that no files have been saved
            return Ok();
        }
    }
}