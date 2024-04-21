using Projecto.Application.Common.Interfaces;
using Stripe;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ISender _sender;
        private readonly IPaymentService _paymentService;
        private readonly IKeyService _keyService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IDataContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CheckOutController(ISender sender, IPaymentService paymentService, IKeyService keyService, IMapper mapper, IEmailService emailService, IDataContext context, UserManager<AppUser> userManager)
        {
            _sender = sender;
            _paymentService = paymentService;
            _keyService = keyService;
            _mapper = mapper;
            _emailService = emailService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CheckOut(int id)
        {
            var gameDto = await _sender.Send(new GetGameQuery(id));
            var game = _mapper.Map<Game>(gameDto);
            var cartItems = new List<CartItem>
            {
                new CartItem { Game = game, Quantity = 1 }
            };
            var domain = "https://localhost:7118/";
            var session = await _paymentService.CreateStripeSession(cartItems, domain + "CheckOut/OrderConfirmation", domain + "CheckOut/OrderCancelled");
            TempData["Session"] = session.Id;
            var gameIds = new List<int>();
            foreach (var cartItem in cartItems)
            {
                while (cartItem.Quantity > 0)
                {
                    gameIds.Add(cartItem.Game.Id);
                    cartItem.Quantity--;
                }
            }
            HttpContext.Session.SetString("GameIds", JsonConvert.SerializeObject(gameIds));
            Response.Headers.Add("location", session.Url);
            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> CheckOutFromCart(List<CartItem> CartItems)
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var domain = "https://localhost:7118/";
            var session = await _paymentService.CreateStripeSession(currentCartItems, domain + "CheckOut/OrderConfirmation", domain + "CheckOut/OrderCancelled");
            TempData["Session"] = session.Id;

            // Store game IDs in the session
            var gameIds = new List<int>();
            foreach (var cartItem in currentCartItems)
            {
                while(cartItem.Quantity > 0)
                {
                    gameIds.Add(cartItem.Game.Id);
                    cartItem.Quantity--;
                }
            }
            HttpContext.Session.SetString("GameIds", JsonConvert.SerializeObject(gameIds));
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
            return RedirectToAction("Index", "Store");
        }

        public async Task<IActionResult> OrderConfirmed()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var service = new SessionService();
            var sessionOptions = new SessionGetOptions();
            sessionOptions.AddExpand("line_items");
            Session session = service.Get(TempData["Session"].ToString(), sessionOptions);
            
            var gameKeys = new List<GameKey>();

            var gameIdsJson = HttpContext.Session.GetString("GameIds");
            var gameIds = gameIdsJson != null
                ? JsonConvert.DeserializeObject<List<int>>(gameIdsJson)
                : new List<int>();

            foreach (var gameId in gameIds)
            {
                var game = await _sender.Send(new GetGameQuery(gameId));
                var gameKey = await _keyService.AssignKeyToUser(User.Identity.Name, gameId);
                gameKeys.Add(new GameKey { Value = gameKey, GameId = gameId, Game = _mapper.Map<Game>(game) });
            }

            var orderConfirmedVM = new OrderConfirmedVM
            {
                OrderId = session.PaymentIntentId,
                AmountPaid = session.AmountTotal / 100,
                GameKeys = gameKeys
            };

            if (session.PaymentStatus == "paid")
            {
                var paymentIntentService = new PaymentIntentService();
                var paymentIntentOptions = new PaymentIntentGetOptions
                {
                    Expand = new List<string> { "payment_method" }
                };
                var paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId, paymentIntentOptions);

                var payment = new Payment
                {
                    UserId = GetUserId(),
                    SessionId = session.Id,
                    Amount = session.AmountTotal / 100,
                    PaymentStatus = session.PaymentStatus,
                    PaymentDate = DateTime.Now,
                    PaymentMethodCountry = paymentIntent.PaymentMethod.Card.Country,
                    PaymentMethodLast4 = paymentIntent.PaymentMethod.Card.Last4
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            // Send email after successful payment
            var cartItems = gameKeys.Select(gk => new CartItem { Game = gk.Game, Quantity = 1 }).ToList();
            await _emailService.SendReceiptEmailAsync(user.Email,session.Id.Substring(10,20),cartItems);
            await _emailService.SendGameKeysEmailAsync(user.Email, gameKeys);
            return View(orderConfirmedVM);
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
