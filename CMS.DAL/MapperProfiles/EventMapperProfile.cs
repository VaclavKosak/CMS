using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<EventEntity, EventEntity>();
        }
    }
}