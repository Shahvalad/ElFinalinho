namespace Projecto.Infrastructure
{
    public static class DependencyInjetion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configurration)
        {
            var key = configurration.GetSection("SendGrid")["ApiKey"];
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IKeyService, KeyService>();
            services.AddSingleton<IEmailService>(new EmailService(key));
            StripeConfiguration.ApiKey = configurration.GetSection("Stripe")["SecretKey"];
            return services;
        }
    }
}
