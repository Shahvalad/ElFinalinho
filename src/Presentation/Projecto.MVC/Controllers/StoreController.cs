using Projecto.Application.Dtos.ReviewDtos;
using Projecto.Application.Features.Games.Queries.GetByName;
using Projecto.Application.Features.Games.Queries.GetGameWithUserFavouriteStatus;
using Projecto.Application.Features.Reviews.Queries.GetAll;
using System.Security.Claims;

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
            return View(games.ToList());
        }

        public async Task<IActionResult> Details(int id, int page = 1)
        {
            string userId = null;

            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var game = await _sender.Send(new GetGameQuery(id));
            if (userId != null)
            {
                game = await _sender.Send(new GetGameWithUserFavouriteStatus(id, userId));
            }

            var paginatedReviewsDto = await _sender.Send(new GetReviewsQuery(id, page));

            var viewModel = new GameDetailsVM
            {
                Game = game,
                NewReview = userId != null ? new ReviewCreateDto(true, "", userId, id) : null,
                PaginatedReviews = new ReviewVM
                {
                    Reviews = paginatedReviewsDto.Reviews,
                    CurrentPage = paginatedReviewsDto.CurrentPage,
                    TotalPages = paginatedReviewsDto.TotalPages,
                }
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var searchResults = await _sender.Send(new GetGameByNameQuery(searchTerm));
            return PartialView("_GameSearchResults", searchResults);
        }
    }
}
