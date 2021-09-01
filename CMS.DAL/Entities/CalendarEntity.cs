using System;

namespace CMS.DAL.Entities
{
    public class CalendarEntity : EntityBase<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
    }
}