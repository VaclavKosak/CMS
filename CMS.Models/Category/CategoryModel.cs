using System;
using System.Collections.Generic;
using CMS.Models.Article;

namespace CMS.Models.Category;

public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<ArticleModel> Article { get; set; }
}