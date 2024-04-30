using Microsoft.AspNetCore.Identity;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Common.Models;
using Projecto.Application.Features.Users.Commands.ConfirmEmail;
using Projecto.Application.Features.Users.Commands.Login;
using Projecto.Application.Features.Users.Commands.Logout;
using Projecto.Application.Features.Users.Commands.Register;
using Projecto.Infrastructure.Services;

namespace Projecto.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISender _sender;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public AccountController(ISender sender, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _sender = sender;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _sender.Send(new UserLoginCommand (model.Username, model.Password, model.RememberMe));

                switch (result.Status)
                {
                    case LoginStatus.Success:
                        return RedirectToAction("Index", "Home");
                    case LoginStatus.EmailNotConfirmed:
                        return RedirectToAction("CheckEmail", "Account");
                    case LoginStatus.Failure:
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        break;
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username, Email = model.Email, Balance = 0, TotalSpendings = 0, MemberSince = DateTime.Now };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    var subject = "Confirm your email";
                    var body = $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>";
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                    return RedirectToAction("CheckEmail", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _sender.Send(new UserConfirmEmailCommand(userId,token));

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return BadRequest(result.Errors);
        }

        public IActionResult CheckEmail()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _sender.Send(new UserLogoutCommand());
            return RedirectToAction("Index", "Home");
        }
    }
}
