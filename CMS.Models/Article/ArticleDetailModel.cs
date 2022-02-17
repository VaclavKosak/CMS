using System;
using System.Collections.Generic;
using CMS.Common.Enums;
using CMS.DAL.Entities;

namespace CMS.Models.Article
{
    public class ArticleDetailModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime PublicationDateTime { get; set; }
        public string Text { get; set; }
        public bool Draft { get; set; }
        public PageType PageType { get; set; }
        
        public ICollection<CategoryEntity> Category { get; set; }
    }
}