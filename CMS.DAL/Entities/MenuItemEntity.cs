using System;

namespace CMS.DAL.Entities;

public class MenuItemEntity : EntityBase<Guid>
{
    public Guid ParentId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public string Url { get; set; }
}