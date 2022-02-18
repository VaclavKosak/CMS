using System;
using System.Collections.Generic;
using CMS.DAL.Entities;
using CMS.Models.Article;

namespace CMS.Models.Category
{
    public class CategoryDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<ArticleListModel> Article { get; set; }
    }
}