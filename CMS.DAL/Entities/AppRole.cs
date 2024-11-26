using System;
using System.ComponentModel.DataAnnotations.Schema;
using CMS.DAL.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CMS.DAL.Entities;

[Table("AspNetRoles")]
public class AppRole : IdentityRole<Guid>, IEntity<Guid>;