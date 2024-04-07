using Projecto.Application.Features.Games.Commands.AddToFavourites;
using Projecto.Application.Features.Games.Commands.RemoveFromFavourites;
using Projecto.Application.Features.Games.Queries.GetFavourites;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class FavouritesController : Controller
    {
        private readonly ISender _sender;

        public FavouritesController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _sender.Send(new GetFavouriteGamesQuery(userId)));
        }

        public async Task<IActionResult> AddToFavourites(int gameId)
        {
            var userId = GetUserId();
            await _sender.Send(new AddGameToFavouritesCommand(userId, gameId));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromFavourites(int gameId)
        {
            var userId = GetUserId();
            await _sender.Send(new RemoveFromFavouritesCommand(gameId, userId));
            return RedirectToAction(nameof(Index));
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }
    }
}
