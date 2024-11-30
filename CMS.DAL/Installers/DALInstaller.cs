using CMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.DAL.Installers;

public class DALInstaller
{
    public static void Install(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<WebDataContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);
        
        serviceCollection.Scan(selector =>
            selector.FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IAppRepository<,>)))
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
    }
}