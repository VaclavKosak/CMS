using Microsoft.Extensions.DependencyInjection;

namespace CMS.Common.Installers;

public interface IInstaller
{
    void Install(IServiceCollection serviceCollection);
}