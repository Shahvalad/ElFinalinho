using Stripe.Checkout;
namespace Projecto.Application.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateStripeSession(List<CartItem> CartItems, string successUrl, string cancelUrl);
    }
}
