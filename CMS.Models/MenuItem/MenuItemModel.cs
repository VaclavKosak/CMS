using System;
using System.Collections.Generic;

namespace CMS.Models.MenuItem;

public class MenuItemModel
{
    public Guid Id { get; set; }
    public Guid ParentId { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public string Url { get; set; }

    public List<MenuItemModel> MenuList { get; set; }
}