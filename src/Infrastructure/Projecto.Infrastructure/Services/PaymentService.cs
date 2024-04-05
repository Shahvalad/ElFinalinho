using Projecto.Application.Dtos.GameDtos;
using Projecto.Application.Services.PaymentService;
using Projecto.Domain.Models;
using Stripe.Checkout;
using System.Web.Mvc;

namespace Projecto.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        public Session CreateStripeSession(List<CartItem> CartItems, string successUrl, string cancelUrl)
        {
            if (CartItems == null || !CartItems.Any())
            {
                throw new ArgumentException("Games list cannot be null or empty.", nameof(CartItems));
            }

            var options = new SessionCreateOptions
            {
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            foreach (var cartItem in CartItems)
            {
                
                var sessionItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cartItem.Game.Name
                        },
                        UnitAmount = (long)(cartItem.Game.Price * 100)
                    },
                    Quantity = cartItem.Quantity
                };
                options.LineItems.Add(sessionItem);
            }

            var service = new SessionService();
            return service.Create(options);
        }

        
    }
}
