
namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PublishersController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public PublishersController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
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

        public IActionResult Create()
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

        public async Task<IActionResult> Edit(GetPublisherQuery query)
        {
            var publisher = await _sender.Send(query);
            return View(_mapper.Map<UpdatePublisherDto>(publisher));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UpdatePublisherDto publisherDto)
        {
            if (ModelState.IsValid)
            {
                var command = new EditPublisherCommand(id, publisherDto);
                await _sender.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(publisherDto);
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
            var command = new DeletePublisherCommand(id);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

    }

}
