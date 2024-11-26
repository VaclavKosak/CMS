using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CMS.Web.Models;

public class FileUploadModel
{
    [Required]
    public IList<IFormFile> FileUpload { get; set; }
    public string Url { get; set; }
}