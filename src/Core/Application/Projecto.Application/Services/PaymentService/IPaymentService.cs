using Stripe.Checkout;
namespace Projecto.Application.Services.PaymentService
{
    public interface IPaymentService
    {
        Session CreateStripeSession(GetGameDto game, string successUrl, string cancelUrl);
    }
}
