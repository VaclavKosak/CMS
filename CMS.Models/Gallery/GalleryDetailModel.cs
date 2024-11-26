using System;
using System.Collections.Generic;
using CMS.Common.Enums;

namespace CMS.Models.Gallery;

public class GalleryDetailModel
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PreviewImg { get; set; }
    public string Url { get; set; }
    public DateTime DateTime { get; set; }
    public SortByType SortBy { get; set; }

    public List<GalleryListModel> GalleryList { get; set; }
    public string ParentUrl { get; set; }
}