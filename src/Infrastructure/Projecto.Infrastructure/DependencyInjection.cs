using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projecto.Application.Services.KeyService;
using Projecto.Application.Services.PaymentService;
using Projecto.Infrastructure.Services;
using Stripe;

namespace Projecto.Infrastructure
{
    public static class DependencyInjetion
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configurration)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IKeyService, KeyService>();
            StripeConfiguration.ApiKey = configurration.GetSection("Stripe")["SecretKey"];
            return services;
        }
    }
}
