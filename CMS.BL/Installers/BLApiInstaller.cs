using CMS.BL.Facades.Interfaces;
using CMS.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.BL.Installers;

public class BLInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(selector =>
            selector.FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
    }
}