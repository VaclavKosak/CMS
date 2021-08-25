using System;
using System.Collections.Generic;

namespace CMS.DAL.Entities
{
    public class EventEntity : EntityBase<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public string Text { get; set; }
    }
}