using Humanizer;
using Microsoft.EntityFrameworkCore;
using Projecto.Application.Dtos.ProfileDtos;
using Projecto.Application.Features.Profile.Commands.Edit;
using Projecto.Application.Features.Profile.Queries;
using Projecto.Persistence.Data;

namespace Projecto.MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ISender _sender;
        public ProfileController(ISender sender)
        {
            _sender = sender;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _sender.Send(new GetProfileQuery(HttpContext.User)));
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _sender.Send(new GetProfileQuery(HttpContext.User));
            return View(new EditProfileDto(user.FirstName, user.LastName, user.Bio,user.Email));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileDto editProfileDto)
        {
            await _sender.Send(new EditProfileCommand(HttpContext.User, editProfileDto));
            return RedirectToAction(nameof(Index));
        }
    }
}
