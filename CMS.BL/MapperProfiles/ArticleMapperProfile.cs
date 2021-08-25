using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Article;

namespace CMS.BL.MapperProfiles
{
    public class ArticleMapperProfile : Profile
    {
        public ArticleMapperProfile()
        {
            CreateMap<ArticleEntity, ArticleListModel>();
            CreateMap<ArticleNewModel, ArticleEntity>();
            CreateMap<ArticleEntity, ArticleDetailModel>();
            CreateMap<ArticleDetailModel, ArticleNewModel>();
            
            CreateMap<ArticleUpdateModel, ArticleEntity>();
            CreateMap<ArticleDetailModel, ArticleUpdateModel>();
        }
    }
}