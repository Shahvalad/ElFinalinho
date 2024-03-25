using Projecto.MVC.Models;
using System.Diagnostics;

namespace Projecto.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
