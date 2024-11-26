using CMS.DAL.Entities.Interfaces;

namespace CMS.DAL.Entities;

public class EntityBase<TId> : IEntity<TId>
{
    public TId Id { get; set; }
}