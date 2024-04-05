using Newtonsoft.Json;
using Projecto.Application.Features.Carts.Commands.AddToCart;
using Projecto.Application.Features.Carts.Commands.RemoveFromCart;

namespace Projecto.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ISender _sender;

        public CartController(ISender sender)
        {
            _sender = sender;
        }

        public IActionResult Index()
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var CurrentCart = new Cart { CartItems = currentCartItems };
            return View(CurrentCart);
        }

        public async Task<IActionResult> AddToCart(int GameId)
        {
            var currentCartItemsJson = HttpContext.Session.GetString("Cart");
            var currentCartItems = currentCartItemsJson != null
                ? JsonConvert.DeserializeObject<List<CartItem>>(currentCartItemsJson)
                : new List<CartItem>();
            var command = new AddToCartCommand { GameId = GameId, CurrentCart = new Cart { CartItems = currentCartItems } };
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

    }
}
