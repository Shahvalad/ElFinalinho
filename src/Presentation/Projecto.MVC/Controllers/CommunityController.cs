using Projecto.Application.Dtos.CommunityDtos;
using Projecto.Application.Features.Communities.Commands.AddPost;
using Projecto.Application.Features.Communities.Commands.DislikePost;
using Projecto.Application.Features.Communities.Commands.LikePost;
using Projecto.Application.Features.Communities.Queries.GetAll;
using Projecto.Application.Features.Communities.Queries.GetById;
using System.Security.Claims;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private readonly ISender _sender;

        public CommunityController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index()
        {
            var communities = await _sender.Send(new GetAllCommunitesQuery());
            return View(communities);
        }

        public async Task<IActionResult> Details(int id)
        {
            var community = await _sender.Send(new GetCommunityByIdQuery(id));
            return View(community.Data);
        }

        [HttpGet]
        public async Task<IActionResult> SearchCommunity(string search)
        {
            var communities = await _sender.Send(new GetAllCommunitesQuery());
            var searchResults = communities.Where(c => c.Name.Contains(search)).ToList();
            return View("Index", searchResults);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, CreateCommunityPostDto communityPostDto)
        {
            await _sender.Send(new AddCommunityPostCommand(communityPostDto, GetUserId(), id));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int postId, int communityId)
        {
            await _sender.Send(new LikeCommunityPostCommand(GetUserId(), postId));
            return RedirectToAction(nameof(Details), new { id = communityId });
        }

        [HttpPost]
        public async Task<IActionResult> DislikePost(int postId, int communityId)
        {
            await _sender.Send(new DislikeCommunityPostCommand(GetUserId(), postId));
            return RedirectToAction(nameof(Details), new { id = communityId });
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }
    }
}
