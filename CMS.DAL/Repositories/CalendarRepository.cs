using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;

namespace CMS.DAL.Repositories
{
    public class CalendarRepository : RepositoryBase<CalendarEntity, Guid>, IAppRepository<CalendarEntity, Guid>
    {
        public CalendarRepository(Func<WebDataContext> contextFactory, IMapper mapper) 
            : base(contextFactory, mapper)
        {
        }
    }
}