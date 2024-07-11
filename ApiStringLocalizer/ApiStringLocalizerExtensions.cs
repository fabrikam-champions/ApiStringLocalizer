using ApiStringLocalizer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiStringLocalizerExtensions
    {
        public static IServiceCollection AddApiStringLocalizer(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            AddApiStringLocalizerServicesWithOptions(services,default);
            return services;
        }
        public static IServiceCollection AddApiStringLocalizer(this IServiceCollection services, Action<ApiStringLocalizerOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            AddApiStringLocalizerServices(services, setupAction);
            return services;
        }
        private static void AddApiStringLocalizerServices(IServiceCollection services, Action<ApiStringLocalizerOptions> setupAction)
        {
            ApiStringLocalizerOptions options = new ApiStringLocalizerOptions();

            setupAction(options);

            services.Configure(setupAction);
            AddApiStringLocalizerServicesWithOptions(services, options);
        }
        private static void AddApiStringLocalizerServicesWithOptions(IServiceCollection services, ApiStringLocalizerOptions options)
        {
            if (!options.DisableApiLocalization)
            {
                services.AddHttpClient();
                services.AddMemoryCache();
                services.AddSingleton<IStringLocalizerFactory, ApiStringLocalizerFactory>();
            }
            services.AddLocalization();
        }
    }
}
