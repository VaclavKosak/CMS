using System;
using System.Threading.Tasks;
using CMS.BL.Installers;
using CMS.DAL;
using CMS.DAL.Entities;
using CMS.DAL.Installers;
using CMS.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources";  });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.AddControllersWithViews();
            services.AddOptions();
            
            services.AddDbContext<WebDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), m => m.MigrationsAssembly("CMS.Web")), ServiceLifetime.Transient);

            new DALInstaller().Install(services);
            new BLInstaller().Install(services);

            services.AddAutoMapper(typeof(DALInstaller), typeof(BLInstaller));
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            
            services.AddSingleton<IEmailSender, EmailSender>();
            
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<WebDataContext>()
                .AddRoles<AppRole>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                // options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // options.User.RequireUniqueEmail = false;
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Home", policy => policy.RequireRole("Admin", "Editor", "User"));
                options.AddPolicy("Article", policy => policy.RequireRole("Admin", "Editor"));
                options.AddPolicy("Calendar", policy => policy.RequireRole("Admin", "Editor"));
                options.AddPolicy("Category", policy => policy.RequireRole("Admin", "Editor"));
                options.AddPolicy("File", policy => policy.RequireRole("Admin", "Editor"));
                options.AddPolicy("Gallery", policy => policy.RequireRole("Admin", "Editor"));
                options.AddPolicy("MenuItem", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserControls", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserRole", policy => policy.RequireRole("Admin"));
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = long.MaxValue; // if don't set default value is: 30 MB
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = long.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var supportedCultures = new[] { "cs", "en" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            // localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

            app.UseRequestLocalization(localizationOptions);
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                    name: "Gallery",
                    pattern: "{controller=Gallery}/{*url}",
                    defaults: new { action = "Details" });
                
                endpoints.MapRazorPages();
            });
            
            UpdateDatabase(app);
            CreateRoles(serviceProvider);
        }
        
        private void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<WebDataContext>();
            if (context != null) context.Database.Migrate();
        }
        
        private void CreateRoles(IServiceProvider serviceProvider)
        {
            // Initializing custom roles 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            // Roles
            string[] roleNames = { "Admin", "Editor", "User"};

            foreach (var roleName in roleNames)
            {
                var roleExist = roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist)
                {
                    var role = new AppRole
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        NormalizedName = roleName.Normalize()
                    };
                    var result = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}