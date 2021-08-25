using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Entities.Interfaces;
using CMS.DAL.Reporitories.Interfaces;

namespace CMS.DAL.Reporitories
{
    public class EventRepository : RepositoryBase<EventEntity, Guid>, IAppRepository<EventEntity, Guid>
    {
        public EventRepository(Func<WebDataContext> contextFactory, IMapper mapper) 
            : base(contextFactory, mapper)
        {
        }
    }
}