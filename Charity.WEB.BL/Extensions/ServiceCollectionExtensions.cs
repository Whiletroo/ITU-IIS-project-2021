//!--Author xkrukh00-- >
using Microsoft.Extensions.DependencyInjection;
using Charity.WEB.BL.Installers;

namespace Charity.WEB.BL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInstaller<TInstaller>(this IServiceCollection serviceCollection, string apiBaseUrl)
            where TInstaller : WebBLInstaller, new()
        {
            var installer = new TInstaller();
            installer.Install(serviceCollection, apiBaseUrl);
        }
    }
}