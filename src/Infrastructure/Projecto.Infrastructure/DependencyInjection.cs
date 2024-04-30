using Projecto.Application.Services.TarotCardService;

namespace Projecto.Infrastructure
{
    public static class DependencyInjetion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetSection("SendGrid")["ApiKey"];
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IKeyService, KeyService>();
            services.AddScoped<ITarotCardService, TarotCardService>();
            services.AddSingleton<IEmailService>(new EmailService(key));
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe")["SecretKey"];
            return services;
        }
    }
}
