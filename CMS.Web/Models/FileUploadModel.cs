using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CMS.Web.Models
{
    public class FileUploadModel
    {
        [Required]
        [Display(Name="File")]
        public List<IFormFile> FileUpload { get; set; }

        [Display(Name="Note")]
        [StringLength(50, MinimumLength = 0)]
        public string Note { get; set; }
    }
}