using Microsoft.Extensions.DependencyInjection;
using SunEngine.Services;

namespace SunEngine.Configuration.AddServices
{
    internal static class AddImagesServicesExtensions
    {
        public static void AddImagesServices(this IServiceCollection services)
        {
            services.AddSingleton<IImagesNamesService, ImagesNamesService>();
            services.AddSingleton<ImagesService>();
        }
    }
}