using Microsoft.Extensions.DependencyInjection;
using Projecto.Application.Common.Behaviours;
using System.Reflection;
namespace Projecto.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg=> cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
