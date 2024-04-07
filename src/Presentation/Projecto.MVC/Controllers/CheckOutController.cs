namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ISender _sender;
        private readonly IPaymentService _paymentService;
        private readonly IKeyService _keyService;
        private readonly IMapper _mapper;
        public CheckOutController(ISender sender, IPaymentService paymentService, IKeyService keyService, IMapper mapper)
        {
            _sender = sender;
            _paymentService = paymentService;
            _keyService = keyService;
            _mapper = mapper;
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
            var session = _paymentService.CreateStripeSession(cartItems, domain + "CheckOut/OrderConfirmation", domain + "CheckOut/OrderCancelled");
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
            var session = _paymentService.CreateStripeSession(currentCartItems, domain + "CheckOut/OrderConfirmation", domain + "CheckOut/OrderCancelled");
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
            var service = new SessionService();
            var sessionOptions = new SessionGetOptions();
            sessionOptions.AddExpand("line_items");
            Session session = service.Get(TempData["Session"].ToString(), sessionOptions);
            var gameKeys = new List<GameKey>();

            // Retrieve game IDs from the session
            var gameIdsJson = HttpContext.Session.GetString("GameIds");
            var gameIds = gameIdsJson != null
                ? JsonConvert.DeserializeObject<List<int>>(gameIdsJson)
                : new List<int>();

            foreach (var gameId in gameIds)
            {
                var game = await _sender.Send(new GetGameQuery(gameId));
                var gameKey = await _keyService.AssignKeyToUser(User.Identity.Name, gameId);
                gameKeys.Add(new GameKey { Value = gameKey, GameId = gameId, Game = _mapper.Map<Game>(game)});
            }

            var orderConfirmedVM = new OrderConfirmedVM
            {
                OrderId = session.PaymentIntentId,
                AmountPaid = session.AmountTotal / 100,
                GameKeys = gameKeys
            };

            return View(orderConfirmedVM);
        }
    }
}
