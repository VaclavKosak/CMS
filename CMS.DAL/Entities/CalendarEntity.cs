using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.DAL.Entities;

public class CalendarEntity : EntityBase<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    [Column(TypeName="timestamp without time zone")]
    public DateTime DateTimeFrom { get; set; }
    [Column(TypeName="timestamp without time zone")]
    public DateTime DateTimeTo { get; set; }
}