using System;

namespace CMS.Models.MenuItem;

public class MenuItemUpdateModel
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public string Url { get; set; }
}