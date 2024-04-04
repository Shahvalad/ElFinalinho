using Microsoft.AspNetCore.Authorization;
using Projecto.Application.Services.KeyService;
using Projecto.Application.Services.PaymentService;
using Projecto.Infrastructure.Services;
using Stripe.Checkout;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ISender _sender;
        private readonly IPaymentService _paymentService;
        private readonly IKeyService _keyService;

        public CheckOutController(ISender sender, IPaymentService paymentService, IKeyService keyService)
        {
            _sender = sender;
            _paymentService = paymentService;
            _keyService = keyService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckOut(int id)
        {
            var game = await _sender.Send(new GetGameQuery(id));
            var domain = "https://localhost:7118/";
            var session = _paymentService.CreateStripeSession(game, domain + "CheckOut/OrderConfirmation", domain + "CheckOut/OrderCancelled");
            TempData["Session"] = session.Id;
            Response.Headers.Add("location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();

            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                return RedirectToAction(nameof(OrderConfirmed));
            }
            return RedirectToAction(nameof(OrderCancelled));
        }

        public IActionResult OrderCancelled()
        {
            return View();
        }

        public IActionResult OrderConfirmed()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            var key = _keyService.AssignKeyToUser(User.Identity.Name);
            var orderConfirmedVM = new OrderConfirmedVM
            {
                OrderId = session.PaymentIntentId,
                AmountPaid = session.AmountTotal / 100,
                Key = key
            };
            return View(orderConfirmedVM);
        }
    }
}
