using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles;

public class CalendarMapperProfile : Profile
{
    public CalendarMapperProfile()
    {
        CreateMap<CalendarEntity, CalendarEntity>();
    }
}