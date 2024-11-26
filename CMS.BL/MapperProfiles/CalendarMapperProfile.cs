using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Calendar;

namespace CMS.BL.MapperProfiles;

public class CalendarMapperProfile : Profile
{
    public CalendarMapperProfile()
    {
        CreateMap<CalendarEntity, CalendarModel>();
        CreateMap<CalendarModel, CalendarEntity>();
        CreateMap<CalendarModel, CalendarModel>();
    }
}