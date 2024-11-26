using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles;

public class ArticleMapperProfile : Profile
{
    public ArticleMapperProfile()
    {
        CreateMap<ArticleEntity, ArticleEntity>();
    }
}