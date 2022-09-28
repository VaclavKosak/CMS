using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common.Enums;
using CMS.DAL.Entities;

namespace CMS.Models.Article
{
    public class ArticleUpdateModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime PublicationDateTime { get; set; }
        public string Text { get; set; }
        public bool Draft { get; set; }
        public PageType PageType { get; set; }
        
        public ICollection<Guid> CategoriesList { get; set; }
    }
}