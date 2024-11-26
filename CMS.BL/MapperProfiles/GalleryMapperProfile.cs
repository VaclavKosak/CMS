using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Gallery;

namespace CMS.BL.MapperProfiles;

public class GalleryMapperProfile : Profile
{
    public GalleryMapperProfile()
    {
        CreateMap<GalleryEntity, GalleryListModel>();
        CreateMap<GalleryNewModel, GalleryEntity>();
        CreateMap<GalleryEntity, GalleryDetailModel>();
        CreateMap<GalleryDetailModel, GalleryNewModel>();

        CreateMap<GalleryUpdateModel, GalleryEntity>();
        CreateMap<GalleryDetailModel, GalleryUpdateModel>();

        CreateMap<GalleryEntity, GalleryUpdateModel>();
    }
}