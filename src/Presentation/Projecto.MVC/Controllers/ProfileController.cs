using Projecto.Application.Dtos.ProfileDtos;
using Projecto.Application.Features.Profile.Commands.ChangeProfilePicture;
using Projecto.Application.Features.Profile.Commands.Edit;
using Projecto.Application.Features.Profile.Queries;
using System.Security.Claims;
using Projecto.Application.Features.Profile.Queries.Get;
using Projecto.Application.Features.Profile.Queries.GetUserProfile;
using Projecto.Application.Features.TarotCards.Queries.GetUserTarotCards;
using Projecto.Application.Common.Interfaces;
using Projecto.Application.Features.TarotCards.Queries.GetTarotCardsOfUser;
using System.Drawing.Text;
using Projecto.Application.Features.Communities.Queries.GetUsersCommunityPosts;
using Projecto.Application.Features.Posts.Queries.GetPostDisplayedOnProfile;
using Projecto.Application.Features.Posts.Commands.SetDisplayedPost;

namespace Projecto.MVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ISender _sender;
        private readonly IDataContext _context;
        public ProfileController(ISender sender, IDataContext context)
        {
            _sender = sender;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userProfile = await _sender.Send(new GetProfileQuery(HttpContext.User));
            var DisplayedPost = await _sender.Send(new GetPostDisplayedOnProfileQuery(GetUserId()));
            var ProfileViewModel = new ProfileViewModel
            {
                DisplayedPost = DisplayedPost,
                User = userProfile
            };
            return View(ProfileViewModel);
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

        [HttpGet]
        public async Task<IActionResult> ViewAllBadges()
        {
            var result = await _sender.Send(new GetTarotCardsOfUserQuery(GetUserId()));
            if(result.Succeeded)
            {
                return View(result.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditBadges()
        {
            var result = await _sender.Send(new GetUserTarotCardsQuery(GetUserId()));
            if (result.Succeeded)
            {
                return View(result.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EditBadges(List<UserTarotCard> userTarotCards)
        {
            if (userTarotCards.Count(c => c.IsDisplayedOnProfile) > 4)
            {
                ModelState.AddModelError("", "You can only display a maximum of 4 cards on your profile.");
                return View(userTarotCards);
            }

            foreach (var userTarotCard in userTarotCards)
            {
                var existingUserTarotCard = _context.UserTarotCards.FirstOrDefault(utc => utc.Id == userTarotCard.Id);
                if (existingUserTarotCard != null)
                {
                    existingUserTarotCard.IsDisplayedOnProfile = userTarotCard.IsDisplayedOnProfile;
                }
            }

            await _context.SaveChangesAsync(cancellationToken: CancellationToken.None);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddCommunityPost()
        {
            var postsOfUser = await _sender.Send(new GetUsersCommunityPostsQuery(GetUserId()));
            return View(postsOfUser);
        }

        [HttpPost]
        public async Task<IActionResult> SetDisplayedPost(List<int> postId)
        {
            var userId = GetUserId();
            var command = new SetDisplayedPostCommand(userId, postId.FirstOrDefault());
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> GetAllGames()
        {
            return View();
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }


    }
}
