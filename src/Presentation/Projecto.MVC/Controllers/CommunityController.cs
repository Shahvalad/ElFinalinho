using Microsoft.AspNetCore.Mvc;

namespace Projecto.MVC.Controllers
{
    public class CommunityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
