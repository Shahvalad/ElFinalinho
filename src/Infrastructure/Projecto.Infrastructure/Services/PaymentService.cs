namespace Projecto.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IEmailService _emailService;

        public PaymentService(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task<Session> CreateStripeSession(List<CartItem> CartItems, string successUrl, string cancelUrl)
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
            Session session = service.Create(options);
            return session;
        }

        
    }
}
