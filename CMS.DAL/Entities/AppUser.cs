using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CMS.DAL.Entities;

[Table("AspNetUsers")]
public class AppUser : IdentityUser<Guid>, IEntity<Guid>
{
    public string ProfilePhoto { get; set; }
}