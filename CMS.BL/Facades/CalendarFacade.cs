using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.Calendar;

namespace CMS.BL.Facades;

public class CalendarFacade(CalendarRepository repository, IMapper mapper)
    : FacadeBase<CalendarModel, CalendarModel, CalendarModel, CalendarModel,
        CalendarRepository, CalendarEntity, Guid>(repository, mapper);