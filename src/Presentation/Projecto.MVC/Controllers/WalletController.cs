using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Projecto.Application.Features.Users.Queries.GetById;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly ISender _sender;
        private readonly UserManager<AppUser> _userManager;
        public WalletController(ISender sender, UserManager<AppUser> userManager)
        {
            _sender = sender;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByIdAsync(GetUserId());
            return View(user);
        }

        private string GetUserId() 
        { 
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
