using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.Common.Enums;

namespace CMS.DAL.Entities;

public class ArticleEntity : EntityBase<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime PublicationDateTime { get; set; }

    public string Text { get; set; }
    public bool Draft { get; set; }
    public PageType PageType { get; set; }

    public ICollection<CategoryEntity> Category { get; set; }
}