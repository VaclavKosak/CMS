using System;
using CMS.Common.Enums;

namespace CMS.Models.Gallery;

public class GalleryNewModel
{
    public Guid ParentId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PreviewImg { get; set; }
    public string Url { get; set; }
    public DateTime DateTime { get; set; }
    public SortByType SortBy { get; set; }
}