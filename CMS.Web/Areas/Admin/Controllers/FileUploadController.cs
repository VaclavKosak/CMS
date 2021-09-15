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


namespace CMS.Web.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("[area]/[controller]/[action]")]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _targetFilePath;

        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L;

        public FileUploadController(ILogger<FileUploadController> logger, IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;

            _targetFilePath = configuration.GetValue<string>("StoredFilesPath");
        }

        [HttpPost]
        // [DisableFormValueModelBinding]
        // [ValidateAntiForgeryToken]
        // [RequestSizeLimit(MaxFileSize)]
        // [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task<IActionResult> UploadLargeFile()
        {
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
                    // Don't trust any file name, file extension, and file data from the request unless you trust them completely
                    // Otherwise, it is very likely to cause problems such as virus uploading, disk filling, etc
                    // In short, it is necessary to restrict and verify the upload
                    // Here, we just use the temporary folder and a random file name

                    // Get the temporary folder, and combine a random file name with it
                    var fileName = Path.GetRandomFileName();
                    var saveToPath = Path.Combine(_webHostEnvironment.WebRootPath, _targetFilePath);

                    await using (var targetStream = System.IO.File.Create(Path.Combine(saveToPath, fileName)))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }

                    Thread imageProcess = new Thread(new ParameterizedThreadStart(ImageHelpers.ResizeImg));
                    imageProcess.Start((saveToPath, fileName));
                }

                section = await reader.ReadNextSectionAsync();
            }

            // If the code runs to this location, it means that no files have been saved
            return Ok();
        }
    }
}