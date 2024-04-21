using Microsoft.AspNetCore.Mvc;
using Projecto.Application.Features.Friends.Commands.SendFriendRequest;
using Projecto.Application.Features.Profile.Queries.GetByName;
using System.Security.Claims;
using Projecto.Application.Features.Friends.Commands.AcceptFriendRequest;
using Projecto.Application.Features.Friends.Commands.DeclineFriendRequest;
using Projecto.Application.Features.Friends.Commands.RemoveFriend;
using Projecto.Application.Features.Friends.Queries.GetAll;
using Projecto.Application.Features.Friends.Queries.GetReceivedFriendshipRequests;
using Projecto.Application.Features.Friends.Queries.GetSentFriendshipRequests;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly ISender _sender;

        public FriendsController(ISender sender)
        {
            _sender = sender;
        }

        public IActionResult Index()
        {
            var friendsVM = new FriendsVM()
            {
                ReceivedFriendshipRequests = _sender.Send(new GetReceivedFriendshipRequestsQuery(GetUserId())).Result.Data,
                SentFriendshipRequests = _sender.Send(new GetSentFriendshipRequestsQuery(GetUserId())).Result.Data,
                Friends = _sender.Send(new GetAllFriendsQuery(GetUserId())).Result.Data
            };
            return View(friendsVM);
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public async Task<IActionResult> AddFriend(string name)
        {
            var command = new SendFriendRequestCommand(name, GetUserId());
            var result = await _sender.Send(command);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            var result = await _sender.Send(new RemoveFriendCommand(GetUserId(), friendId));
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AcceptFriendRequest(int requestId)
        {
            var result = await _sender.Send(new AcceptFriendRequestCommand(requestId));
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeclineFriendRequest(int requestId)
        {
            var result = await _sender.Send(new DeclineFriendRequestCommand(requestId));
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = result.SuccessMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault();
            }
            return RedirectToAction(nameof(Index));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }

    }
}
