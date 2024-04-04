using Projecto.Application.Features.Games.Queries.GetByName;

//TO DO : IF THERE IS NO STOCK OF THE GAME, THEN THE USER SHOULD NOT BE ABLE TO ADD IT TO THE CART OR BUY IT
namespace Projecto.MVC.Controllers
{
    public class StoreController : Controller
    {
        private readonly ISender _sender;

        public StoreController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _sender.Send(new GetGamesQuery());
            return View(games);
        }

        public async Task<IActionResult> Details(int id)
        {
            var game = await _sender.Send(new GetGameQuery(id));
            return View(game);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var searchResults = await _sender.Send(new GetGameByNameQuery(searchTerm));
            return PartialView("_GameSearchResults", searchResults);
        }
    }
}
