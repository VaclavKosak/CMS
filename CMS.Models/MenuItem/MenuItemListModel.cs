using System;

namespace CMS.Models.MenuItem
{
    public class MenuItemListModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
    }
}