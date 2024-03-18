using Microsoft.AspNetCore.Mvc;
using Projecto.Application.Dtos.Publisher;
using Projecto.Application.Features.Publishers.Queries.GetPublishers;
namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PublishersController : Controller
    {
        private readonly ISender _sender;

        public PublishersController(ISender sender)
        {
            _sender = sender;
        }

        public async Task<IActionResult> Index(GetPublishersQuery query)
        {
            var publishers = await _sender.Send(query);
            return View(publishers);
        }

        public async Task<IActionResult> Details(GetPublisherQuery query)
        {
            var publisher = await _sender.Send(query);
            return View(publisher);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePublisherDto publisherDto)
        {
            if (ModelState.IsValid)
            {
                var command = new CreatePublisherCommand(publisherDto);
                await _sender.Send(command);
                return RedirectToAction("Index");
            }
            return View(publisherDto);
        }
    }

}
