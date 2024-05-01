using Microsoft.Extensions.Options;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Common.Options;
using Projecto.Application.Features.Users.Commands.UpdateBalance;
using Projecto.Application.Features.Users.Queries.GetBalance;
using Projecto.Application.Services.TarotCardService;
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
        private readonly ITarotCardService _tarotCardService;
        private readonly string _domain;
        public CheckOutController(ISender sender, IPaymentService paymentService, IKeyService keyService, IMapper mapper, IEmailService emailService, IDataContext context, UserManager<AppUser> userManager, IOptions<WebsiteOptions> websiteOptions, ITarotCardService tarotCardService)
        {
            _sender = sender;
            _paymentService = paymentService;
            _keyService = keyService;
            _mapper = mapper;
            _emailService = emailService;
            _context = context;
            _userManager = userManager;
            _domain = websiteOptions.Value.Domain;
            _tarotCardService = tarotCardService;
        }


        public async Task<IActionResult> CheckOut(int id)
        {
            var gameDto = await _sender.Send(new GetGameQuery(id));
            var game = _mapper.Map<Game>(gameDto);
            var cartItems = new List<CartItem>
            {
                new CartItem { Game = game, Quantity = 1 }
            };
            return await ProcessCheckout(cartItems);
        }

        public async Task<IActionResult> CheckOutFromCart(List<CartItem> CartItems)
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            return await ProcessCheckout(currentCartItems);
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

            var cartItemsJson = HttpContext.Session.GetString("CartItems");
            var cartItems = cartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson)
                : new List<CartItem>();

            foreach (var cartItem in cartItems)
            {
                var game = await _sender.Send(new GetGameQuery(cartItem.Game.Id));
                for (var i = 0; i < cartItem.Quantity; i++)
                {
                    var gameKey = await _keyService.AssignKeyToUser(User.Identity.Name, cartItem.Game.Id);
                    gameKeys.Add(new GameKey { Value = gameKey, GameId = cartItem.Game.Id, Game = _mapper.Map<Game>(game) });
                }
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
                if (session.AmountTotal / 100 > 30)
                {
                    var droppedCard = await _tarotCardService.AssignRandomCardToUser(user.Id);
                    orderConfirmedVM.DroppedCard = droppedCard;
                }
            }
            var cartItemsForEmail = gameKeys.Select(gk => new CartItem { Game = gk.Game, Quantity = 1 }).ToList();
            await _emailService.SendReceiptEmailAsync(user.Email, session.Id.Substring(10, 20), cartItemsForEmail);
            await _emailService.SendGameKeysEmailAsync(user.Email, gameKeys);
            return View(orderConfirmedVM);
        }
        public async Task<IActionResult> PayFromWallet()
        {
            var userBalance = await _sender.Send(new GetUserBalanceQuery(GetUserId()));

            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var currentCart = new Cart { CartItems = currentCartItems };

            if (userBalance < currentCart.TotalPrice)
            {
                TempData["Error"] = "You don't have enough balance to pay for these items.";
                return RedirectToAction("Index", "Cart");
            }

            var updateUserBalanceCommand = new UpdateUserBalanceCommand(GetUserId(), userBalance - currentCart.TotalPrice);
            await _sender.Send(updateUserBalanceCommand);

            HttpContext.Session.Remove("Cart");

            var gameKeys = await ProcessOrder(currentCartItems);

            return View("DisplayGameKeys", gameKeys);
        }

        public IActionResult DisplayGameKeys(List<GameKey> gameKeys)
        {
            return View(gameKeys);
        }
        private async Task<IActionResult> ProcessCheckout(List<CartItem> cartItems)
        {
            var session = await _paymentService.CreateStripeSession(cartItems, _domain + "CheckOut/OrderConfirmation", _domain + "CheckOut/OrderCancelled");
            TempData["Session"] = session.Id;
            HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartItems));
            Response.Headers.Add("location", session.Url);
            return new StatusCodeResult(303);
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private  async Task<List<GameKey>> ProcessOrder(List<CartItem> cartItems)
        {
            var gameKeys = new List<GameKey>();

            foreach (var cartItem in cartItems)
            {
                var game = await _sender.Send(new GetGameQuery(cartItem.Game.Id));
                for (var i = 0; i < cartItem.Quantity; i++)
                {
                    var gameKey = await _keyService.AssignKeyToUser(User.Identity.Name, cartItem.Game.Id);
                    gameKeys.Add(new GameKey { Value = gameKey, GameId = cartItem.Game.Id, Game = _mapper.Map<Game>(game) });
                }
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cartItemsForEmail = gameKeys.Select(gk => new CartItem { Game = gk.Game, Quantity = 1 }).ToList();
            await _emailService.SendReceiptEmailAsync(user.Email, "Order ID", cartItemsForEmail);
            await _emailService.SendGameKeysEmailAsync(user.Email, gameKeys);
            return gameKeys;
            
        }

    }
}
