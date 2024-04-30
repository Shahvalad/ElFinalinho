using Microsoft.AspNetCore.Mvc;
using Projecto.Application.Features.AdminDashboardUser.Commands.CreateAdmin;
using Projecto.Application.Features.AdminDashboardUser.Commands.CreateModerator;
using Projecto.Application.Features.AdminDashboardUser.Commands.Login;
using Projecto.Application.Features.AdminDashboardUser.Commands.Logout;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly ISender _sender;

        public AccountsController(ISender sender)
        {
            _sender = sender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateModeratorCommand command)
        {
            var result = await _sender.Send(command);
            if (result.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _sender.Send(new LogoutCommand());
                }
                return RedirectToAction("Index", "Dashboards", new { Area = "Admin" });
            }
            ModelState.AddModelError("", string.Join("; ", result.Errors));
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _sender.Send(command);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboards", new { Area = "Admin" });
            }
            ModelState.AddModelError("", string.Join("; ", result.Errors));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutCommand();
            var result = await _sender.Send(command);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            ModelState.AddModelError("", string.Join("; ", result.Errors));
            return View("Index");
        }
    }
}
