using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories;
using CMS.Models.Gallery;

namespace CMS.BL.Facades
{
    public class GalleryFacade : FacadeBase<GalleryListModel, GalleryDetailModel, GalleryNewModel, GalleryUpdateModel, 
        GalleryRepository, GalleryEntity, Guid>
    {
        public GalleryFacade(GalleryRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<List<GalleryListModel>> GetAll(Guid parentId)
        {
            return Mapper.Map<List<GalleryListModel>>(await Repository.GetAll(parentId));
        }
        
        public virtual async Task<GalleryDetailModel> GetByUrl(string url)
        {
            var cleanUrl = url.Trim(' ', '/');

            var urlParts = cleanUrl.Split('/');
            var parentId = Guid.Empty;

            var entity = new GalleryEntity();
            
            foreach (var urlPart in urlParts)
            {
                entity = await Repository.GetByUrl(urlPart, parentId);
                if (entity == null)
                {
                    return null;
                }
                parentId = entity.ParentId;
            }
            
            var detailData = Mapper.Map<GalleryDetailModel>(entity);
            detailData.GalleryList = await GetAll(detailData.Id);
            detailData.ParentUrl = cleanUrl;
            return detailData;
        }
    }
}