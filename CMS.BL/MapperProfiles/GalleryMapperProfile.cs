using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Gallery;

namespace CMS.BL.MapperProfiles
{
    public class GalleryMapperProfile : Profile
    {
        public GalleryMapperProfile()
        {
            CreateMap<GalleryEntity, GalleryDetailModel>();
            CreateMap<GalleryEntity, GalleryListModel>();
            CreateMap<GalleryDetailModel, GalleryEntity>();
            CreateMap<GalleryNewModel, GalleryEntity>();
            
            CreateMap<GalleryUpdateModel, ArticleEntity>();
            CreateMap<GalleryDetailModel, GalleryUpdateModel>();
        }
    }
}