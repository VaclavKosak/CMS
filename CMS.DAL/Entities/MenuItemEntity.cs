using System;
using CMS.Common.Enums;

namespace CMS.DAL.Entities
{
    public class MenuItemEntity : EntityBase<Guid>
    {
        public string Title { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
    }
}