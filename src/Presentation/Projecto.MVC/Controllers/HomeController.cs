using System.Security.Claims;
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

        public IActionResult Index()
        {
            return View();
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }
    }
}