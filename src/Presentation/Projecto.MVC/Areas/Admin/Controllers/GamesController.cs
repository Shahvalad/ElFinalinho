namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GamesController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public GamesController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _sender.Send(new GetGamesQuery());
            return View(games);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var game = await _sender.Send(new GetGameQuery(id));
            if (game is null)
            {
                return NotFound();
            }
            return View(game);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["DeveloperId"] = new SelectList(await _sender.Send(new GetDevelopersQuery()), "Id", "Name");
            ViewData["PublisherId"] = new SelectList(await _sender.Send(new GetPublishersQuery()), "Id", "Name");
            ViewBag.Genres = new SelectList(await _sender.Send(new GetGenresQuery()), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["DeveloperId"] = new SelectList(await _sender.Send(new GetDevelopersQuery()), "Id", "Name");
                ViewData["PublisherId"] = new SelectList(await _sender.Send(new GetPublishersQuery()), "Id", "Name");
                ViewBag.Genres = new SelectList(await _sender.Send(new GetGenresQuery()), "Id", "Name");
                return View(gameDto);
            }
            var command = new CreateGameCommand(gameDto);
            await _sender.Send(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            var game = await _sender.Send(new GetGameQuery(Id));
            if (game is null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(await _sender.Send(new GetDevelopersQuery()), "Id", "Name");
            ViewData["PublisherId"] = new SelectList(await _sender.Send(new GetPublishersQuery()), "Id", "Name");
            ViewBag.Genres = new SelectList(await _sender.Send(new GetGenresQuery()), "Id", "Name");
            var updateGameDto = _mapper.Map<UpdateGameDto>(game);
            updateGameDto.Images = game.Images.Select(i=>i.ImageFile).ToList();
            updateGameDto.ImageFileNames = game.Images.Where(im => im.IsCoverImage == false).Select(i => i.FileName).ToList();
            updateGameDto.CoverImage = game.Images.FirstOrDefault(i => i.IsCoverImage)?.ImageFile;
            updateGameDto.CoverImageFileName = game.Images.FirstOrDefault(i => i.IsCoverImage)?.FileName;
            updateGameDto.SelectedGenres = game.GameGenres.Select(gg => gg.GenreId).ToList();
            return View(updateGameDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, UpdateGameDto gameDto)
        {
            if (id is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ViewData["DeveloperId"] = new SelectList(await _sender.Send(new GetDevelopersQuery()), "Id", "Name");
                ViewData["PublisherId"] = new SelectList(await _sender.Send(new GetPublishersQuery()), "Id", "Name");
                ViewBag.Genres = new SelectList(await _sender.Send(new GetGenresQuery()), "Id", "Name");
                return View(gameDto);
            }
            var command = new EditGameCommand(id, gameDto);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var game = await _sender.Send(new GetGameQuery(id));
            if (game is null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            await _sender.Send(new DeleteGameCommand(id));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveImage(string fileName)
        {
           var id = await _sender.Send(new DeleteGameImageCommand(fileName));
           return RedirectToAction("Edit", new { id });
        }
    }
}
