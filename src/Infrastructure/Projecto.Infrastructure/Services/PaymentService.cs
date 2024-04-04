using Projecto.Application.Dtos.GameDtos;
using Projecto.Application.Services.PaymentService;
using Projecto.Domain.Models;
using Stripe.Checkout;
using System.Web.Mvc;

namespace Projecto.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        public Session CreateStripeSession(GetGameDto game, string successUrl, string cancelUrl)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            var sessionItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = game.Name
                    },
                    UnitAmount = (long)(game.Price * 100)
                },
                Quantity = 1
            };
            options.LineItems.Add(sessionItem);

            var service = new SessionService();
            return service.Create(options);
        }

        
    }
}
