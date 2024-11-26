using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using CMS.DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CMS.DAL.Entities;

[Table("AspNetUserRoles")]
public class AppUserRole : IdentityUserRole<Guid>, IEntity<Guid>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
    [AllowNull]
    public Guid Id { get; set; }
}