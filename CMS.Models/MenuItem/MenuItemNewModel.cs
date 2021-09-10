using System;

namespace CMS.Models.MenuItem
{
    public class MenuItemNewModel
    {
        public Guid ParentId { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
    }
}