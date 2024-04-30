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

            services.AddAuthentication()
                .AddCookie("UserAuth", options =>
            {
                options.LoginPath = "/Accounts/Login";
            })
                .AddCookie("AdminAuth", options =>
            {
                options.LoginPath = "/Admin/Accounts/Login";
            })
                .AddCookie("ModeratorAuth", options =>
            {
                options.LoginPath = "/Moderator/Accounts/Login";
            });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add("UserAuth");
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add("AdminAuth");
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("AdminOrModeratorPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add("AdminAuth");
                    policy.AuthenticationSchemes.Add("ModeratorAuth");
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser>>();
            services.AddControllersWithViews();
        }
    }
}
