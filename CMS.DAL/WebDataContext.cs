using System;
using CMS.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL
{
    public class WebDataContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>,
        AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public WebDataContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public virtual DbSet<ArticleEntity> Article { get; set; }
        public virtual DbSet<CategoryEntity> Category { get; set; }
        public virtual DbSet<GalleryEntity> Gallery { get; set; }
        public virtual DbSet<MenuItemEntity> MenuItem { get; set; }
        public virtual DbSet<CalendarEntity> Calendar { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}