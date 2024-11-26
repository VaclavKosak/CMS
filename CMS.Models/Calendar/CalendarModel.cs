using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Models.Calendar;

public class CalendarModel
{
    public Guid Id { get; set; }

    [Required] public string Title { get; set; }

    public string Description { get; set; }
    public DateTime DateTimeFrom { get; set; }
    public DateTime DateTimeTo { get; set; }
}