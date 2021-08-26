using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Calendar;

namespace CMS.BL.MapperProfiles
{
    public class CalendarMapperProfile : Profile
    {
        public CalendarMapperProfile()
        {
            CreateMap<CalendarEntity, CalendarListModel>();
            CreateMap<CalendarNewModel, CalendarEntity>();
            CreateMap<CalendarEntity, CalendarDetailModel>();
            CreateMap<CalendarDetailModel, CalendarNewModel>();
            
            CreateMap<CalendarUpdateModel, CalendarEntity>();
            CreateMap<CalendarDetailModel, CalendarUpdateModel>();
        }
    }
}