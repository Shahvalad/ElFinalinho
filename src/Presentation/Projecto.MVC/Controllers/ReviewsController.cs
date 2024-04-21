using Ganss.Xss;
using Projecto.Application.Dtos.ReviewDtos;
using Projecto.Application.Features.Reviews.Commands.Create;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ISender _sender;

        public ReviewsController(ISender sender)
        {
            _sender = sender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateDto reviewDto)
        {
            await _sender.Send(new CreateReviewCommand(reviewDto.IsLiked, reviewDto.Comment, reviewDto.UserId, reviewDto.GameId));
            return RedirectToAction("Details","Store", new { id = reviewDto.GameId });
        }
    }
}
