using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Gallery;

namespace CMS.BL.MapperProfiles;

public class GalleryMapperProfile : Profile
{
    public GalleryMapperProfile()
    {
        CreateMap<GalleryEntity, GalleryModel>();
        CreateMap<GalleryModel, GalleryEntity>();
        CreateMap<GalleryEntity, GalleryModel>();
        CreateMap<GalleryModel, GalleryModel>();

        CreateMap<GalleryModel, GalleryEntity>();
        CreateMap<GalleryModel, GalleryModel>();

        CreateMap<GalleryEntity, GalleryModel>();
    }
}