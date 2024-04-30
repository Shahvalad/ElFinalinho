using Microsoft.Extensions.DependencyInjection;
using Projecto.Application.Common.Behaviours;
using Projecto.Application.Common.Options;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Projecto.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<WebsiteOptions>(configuration.GetSection("WebsiteOptions"));

            return services;
        }
    }
}