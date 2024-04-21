using Projecto.Application.Dtos.ProfileDtos;
using Projecto.Application.Features.Profile.Commands.ChangeProfilePicture;
using Projecto.Application.Features.Profile.Commands.Edit;
using Projecto.Application.Features.Profile.Queries;
using System.Security.Claims;
using Projecto.Application.Features.Profile.Queries.Get;
using Projecto.Application.Features.Profile.Queries.GetUserProfile;

namespace Projecto.MVC.Controllers
{
    [Authorize]
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
            return View(new EditProfileDto(user.FirstName, user.LastName, user.Bio));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileDto editProfileDto)
        {
            var validator = new EditProfileCommandValidator();
            var command = new EditProfileCommand(HttpContext.User, editProfileDto);
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                return View(editProfileDto);
            }
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePicture(IFormFile file)
        {
            var result = await _sender.Send(new ChangeProfilePictureCommand(GetUserId(), file));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UserProfile(string id)
        {
            var result = await _sender.Send(new GetUserProfileQuery(id));
            return View(result.Data);
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }


    }
}
