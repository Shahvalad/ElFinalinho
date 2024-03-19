namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DevelopersController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public DevelopersController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(GetDevelopersQuery query)
        {
            return View(await _sender.Send(query));
        }
        public async Task<IActionResult> Details(int? id)
        {
            var developer = await _sender.Send(new GetDeveloperQuery(id));
            return View(developer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDeveloperDto developerDto)
        {
            var command = new CreateDeveloperCommand(developerDto);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var developer = await _sender.Send(new GetDeveloperQuery(id));
            return View(_mapper.Map<UpdateDeveloperDto>(developer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UpdateDeveloperDto developerDto)
        {
            var command = new EditDeveloperCommand(id, developerDto);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var developer = await _sender.Send(new GetDeveloperQuery(id));
            return View(developer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var command = new DeleteDeveloperCommand(id);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
