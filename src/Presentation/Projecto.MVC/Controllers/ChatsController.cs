using Projecto.Application.Features.Messages.Queries.GetMessages;
using Projecto.Application.Features.Profile.Queries.GetWithMessageStatus;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class ChatsController : Controller
    {
        private readonly ISender _sender;

        public ChatsController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _sender.Send(new GetProfileWithMessageStatusQuery(GetUserId()));
            return View(result.Data);
        }

        public async Task<IActionResult> LoadChat(string recipientUsername)
        {
            var listOfMessageDtos = await _sender.Send(new GetMessagesQuery(GetUserName(), recipientUsername));
            return PartialView("_ChatPartial", listOfMessageDtos);
        }

        private string GetUserName()
        {
            return User.Identity.Name;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }


}
