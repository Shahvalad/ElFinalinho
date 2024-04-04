using Microsoft.AspNetCore.Identity;

namespace Projecto.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.AddInterceptors(new TimestampInterceptor());
            });
            services.AddScoped<IDataContext, DataContext>();

            services.AddIdentity<AppUser, IdentityRole>(
                options=>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireUppercase = true;
                })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
