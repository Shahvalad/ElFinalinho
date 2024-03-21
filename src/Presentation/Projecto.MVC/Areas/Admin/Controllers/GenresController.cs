using Projecto.Application.Dtos.GenreDtos;

namespace fin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenresController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public GenresController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(GetGenresQuery query)
        {
            var genres = await _sender.Send(query);
            return View(genres);
        }

        public async Task<IActionResult> Details(GetGenreQuery query)
        {
            var genre = await _sender.Send(query);
            return View(genre);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGenreDto genreDto)
        {
            if (ModelState.IsValid) return View(genreDto);
            var command = new CreateGenreCommand(genreDto);
            await _sender.Send(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(GetGenreQuery query)
        {
            var genre = await _sender.Send(query);
            return View(_mapper.Map<UpdateGenreDto>(genre));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UpdateGenreDto genreDto)
        {
            if (!ModelState.IsValid) return View(genreDto);
            var command = new EditGenreCommand(id, genreDto);
            genreDto = await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(GetGenreQuery query)
        {
            return View(await _sender.Send(query));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null) return NotFound();
            await _sender.Send(new DeleteGenreCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
