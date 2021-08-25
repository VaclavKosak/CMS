using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles
{
    public class GalleryMapperProfile : Profile
    {
        public GalleryMapperProfile()
        {
            CreateMap<GalleryEntity, GalleryEntity>();
        }
    }
}