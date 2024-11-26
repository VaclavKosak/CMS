using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;

namespace CMS.DAL.Repositories;

public class CalendarRepository(Func<WebDataContext> contextFactory, IMapper mapper)
    : RepositoryBase<CalendarEntity, Guid>(contextFactory, mapper), IAppRepository<CalendarEntity, Guid>;