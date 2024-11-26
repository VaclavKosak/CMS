using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.Calendar;

namespace CMS.BL.Facades;

public class CalendarFacade(CalendarRepository repository, IMapper mapper)
    : FacadeBase<CalendarListModel, CalendarDetailModel, CalendarNewModel, CalendarUpdateModel,
        CalendarRepository, CalendarEntity, Guid>(repository, mapper);