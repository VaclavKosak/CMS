using CMS.Common.Installers;
using CMS.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.DAL.Installers
{
    public class DALInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(selector =>
                selector.FromCallingAssembly()
                    .AddClasses(classes => classes.AssignableTo(typeof(IAppRepository<,>)))
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
            );
        }
    }
}