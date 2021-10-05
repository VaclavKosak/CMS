using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                parentId = entity.Id;
            }
            
            var detailData = Mapper.Map<GalleryDetailModel>(entity);
            detailData.GalleryList = await GetAll(detailData.Id);
            detailData.ParentUrl = cleanUrl;
            return detailData;
        }

        public async Task<(string, string[])> GetParentUrl(Guid id)
        {
            var item = await Repository.GetById(id);
            var urlList = new List<string>();
            if (item == null)
            {
                return ("", urlList.ToArray());
            }
            
            var url = item.Url;
            urlList.Add(url);
            while (item.ParentId != Guid.Empty)
            {
                item = await Repository.GetById(item.ParentId);
                url = Path.Combine(item.Url, url);
                urlList.Add(url);
            }

            urlList.Reverse();

            return (url.Replace("\\", "/"), urlList.ToArray());
        }
    }
}