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
            return services;
        }
    }
}
