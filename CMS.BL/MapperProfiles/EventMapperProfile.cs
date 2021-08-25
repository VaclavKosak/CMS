using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Event;

namespace CMS.BL.MapperProfiles
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<EventEntity, EventListModel>();
            CreateMap<EventNewModel, EventEntity>();
            CreateMap<EventEntity, EventDetailModel>();
            CreateMap<EventDetailModel, EventNewModel>();
            
            CreateMap<EventUpdateModel, EventEntity>();
            CreateMap<EventDetailModel, EventUpdateModel>();
        }
    }
}