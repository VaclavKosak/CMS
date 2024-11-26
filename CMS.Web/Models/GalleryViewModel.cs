using System.Collections.Generic;
using CMS.Models.Gallery;

namespace CMS.Web.Models;

public class GalleryViewModel
{
    public IEnumerable<GalleryListModel> GalleryList { get; set; }
    public GalleryDetailModel GalleryDetail { get; set; }
    public FileUploadModel Files { get; set; }
    public List<(string, string, string)> FilesPath { get; set; }
}