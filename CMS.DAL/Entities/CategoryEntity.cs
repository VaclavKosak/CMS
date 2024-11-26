using System;
using System.Collections.Generic;

namespace CMS.DAL.Entities;

public class CategoryEntity : EntityBase<Guid>
{
    public string Name { get; set; }
        
    public ICollection<ArticleEntity> Article { get; set; }
}