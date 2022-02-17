using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.Calendar;

namespace CMS.BL.Facades
{
    public class CalendarFacade : FacadeBase<CalendarListModel, CalendarDetailModel, CalendarNewModel, CalendarUpdateModel, 
        CalendarRepository, CalendarEntity, Guid>
    {
        public CalendarFacade(CalendarRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}