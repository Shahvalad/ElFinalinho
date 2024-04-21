using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Projecto.Application.Features.Profile.Queries.Get;

namespace Projecto.MVC.Controllers
{
    public class BaseController : Controller
    {
        private readonly ISender _sender;
        public BaseController(ISender sender)
        {
            _sender = sender;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = _sender.Send(new GetProfileQuery(HttpContext.User)).GetAwaiter().GetResult();
                var layoutModel = new LayoutVM()
                {
                    ProfilePictureName = user.ProfilePicture ?? "",
                };
                ViewData["LayoutModel"] = layoutModel;
            }
        }


    }
}
