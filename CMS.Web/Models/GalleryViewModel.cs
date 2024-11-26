using System.Collections.Generic;
using CMS.Models.Gallery;

namespace CMS.Web.Models;

public class GalleryViewModel
{
    public IEnumerable<GalleryModel> GalleryList { get; set; }
    public GalleryModel Gallery { get; set; }
    public FileUploadModel Files { get; set; }
    public List<(string, string, string)> FilesPath { get; set; }
}