using Microsoft.AspNetCore.Mvc;

namespace Projecto.MVC.Controllers
{
    public class MarketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
