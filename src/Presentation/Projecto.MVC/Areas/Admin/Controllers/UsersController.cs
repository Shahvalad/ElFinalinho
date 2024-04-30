using Microsoft.AspNetCore.Mvc;
using Projecto.Application.Features.Users.Queries.GetAll;
using Projecto.Application.Features.Users.Queries.GetById;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrModeratorPolicy")]
    public class UsersController : Controller
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index(List<string> roles = null)
        {
            roles = roles ?? new List<string> { "Admin", "Moderator", "User" };
            var users = await _sender.Send(new GetAllUsersQuery(roles));
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _sender.Send(new GetUserByIdQuery(id));
            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _sender.Send(new GetUserByIdQuery(id));
            return View(user);
        }
    }
}
