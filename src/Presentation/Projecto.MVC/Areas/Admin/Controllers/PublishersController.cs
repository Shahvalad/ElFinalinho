using Projecto.Application.Dtos.PublisherDtos;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrModeratorPolicy")]

    public class PublishersController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public PublishersController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var publishers = await _sender.Send(new GetPublishersQuery());
            return View(publishers);
        }

        public async Task<IActionResult> Details(GetPublisherQuery query)
        {
            var publisher = await _sender.Send(query);
            return View(publisher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePublisherDto publisherDto)
        {
            if (!ModelState.IsValid) return View(publisherDto);
            var command = new CreatePublisherCommand(publisherDto);
            await _sender.Send(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(GetPublisherQuery query)
        {
            var publisher = await _sender.Send(query);
            return View(_mapper.Map<UpdatePublisherDto>(publisher));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UpdatePublisherDto publisherDto)
        {
            if (id is null) return NotFound();
            if (!ModelState.IsValid) return View(publisherDto);
            var command = new EditPublisherCommand(id, publisherDto);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(GetPublisherQuery query)
        {
            var publisher = await _sender.Send(query);
            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null) return NotFound();
            var command = new DeletePublisherCommand(id);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

    }

}
