using Projecto.Application.Features.Carts.Commands.AddToCart;
using Projecto.Application.Features.Carts.Commands.RemoveFromCart;
using Projecto.Application.Features.Users.Queries.GetBalance;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ISender _sender;
        public CartController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var CurrentCart = new Cart { CartItems = currentCartItems };
            var userBalance = await _sender.Send(new GetUserBalanceQuery(GetUserId()));
            var cartVM = new CartVM { Cart = CurrentCart, UserBalance = userBalance };
            return View(cartVM);
        }

        public async Task<IActionResult> AddToCart(int GameId, int quantity)
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var command = new AddToCartCommand { GameId = GameId, CurrentCart = new Cart { CartItems = currentCartItems }, Quantity = quantity};
            await _sender.Send(command);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(command.CurrentCart.CartItems));
            return RedirectToAction("Index","Store");
        }

        public async Task<IActionResult> RemoveFromCart(int GameId)
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var command = new RemoveFromCartCommand { GameId = GameId, CurrentCart = new Cart { CartItems = currentCartItems } };
            await _sender.Send(command);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(command.CurrentCart.CartItems));
            return RedirectToAction("Index");
        }


        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
