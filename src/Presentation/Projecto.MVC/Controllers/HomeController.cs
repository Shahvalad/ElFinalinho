using System.Security.Claims;
using Projecto.Application.Features.Games.Queries.GetPopulars;
using Projecto.Application.Features.Games.Queries.GetRandomGamesQuery;
using Projecto.Application.Features.Games.Queries.RecommendGames;
using Projecto.Application.Features.Profile.Queries.Get;

namespace Projecto.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISender _sender;
        public HomeController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var popularGames = await _sender.Send(new GetPopularGamesQuery());
            var recommendedGames = User.Identity.IsAuthenticated
                ? await _sender.Send(new RandomRecommendGamesQuery(GetUserId()))
                : await _sender.Send(new GetRandomGamesQuery());

            HomeVM homeVM = new HomeVM()
            {
                PopularGames = popularGames,
                RecommendedGames = recommendedGames
            };
            return View(homeVM);
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }
    }
}