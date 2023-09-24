using System.Collections.Generic;
using CMS.Models.Article;

namespace CMS.Web.Areas.Admin.Models;

public class PagePartsItemModel
{
    public string PartName { get; set; }
    public IList<ArticleDetailModel> Articles { get; set; } 
}