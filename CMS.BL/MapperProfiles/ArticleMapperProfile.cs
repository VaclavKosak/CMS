using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Article;

namespace CMS.BL.MapperProfiles;

public class ArticleMapperProfile : Profile
{
    public ArticleMapperProfile()
    {
        CreateMap<ArticleEntity, ArticleModel>();
        CreateMap<ArticleModel, ArticleEntity>();
        CreateMap<ArticleModel, ArticleModel>();
    }
}