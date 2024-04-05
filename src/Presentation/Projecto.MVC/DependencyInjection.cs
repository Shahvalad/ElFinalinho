namespace Projecto.MVC
{
    public static class DependencyInjection
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.IOTimeout = TimeSpan.FromHours(1);
            });
            services.AddControllersWithViews();
        }
    }
}
