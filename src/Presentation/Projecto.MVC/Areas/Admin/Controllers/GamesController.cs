using ExcelDataReader;
using Projecto.Application.Common.Models;
using Projecto.Application.Features.Games.Commands.AddKeys;

namespace Projecto.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GamesController : Controller
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public GamesController(ISender sender, IMapper mapper, IWebHostEnvironment environment)
        {
            _sender = sender;
            _mapper = mapper;
            _environment = environment;
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
            updateGameDto.Images = game.Images?.Select(i=>i.ImageFile).ToList();
            updateGameDto.ImageFileNames = game.Images.Where(im => im.IsCoverImage == false).Select(i => i.FileName).ToList();
            updateGameDto.CoverImage = game.Images?.FirstOrDefault(i => i.IsCoverImage)?.ImageFile;
            updateGameDto.CoverImageFileName = game.Images?.FirstOrDefault(i => i.IsCoverImage)?.FileName;
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

        public async Task<IActionResult> AddKeys(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var game = await _sender.Send(new GetGameQuery(id));
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddKeys(int? id, string keys, IFormFile? file)
        {
            if(file is not null)
            {
                keys = await ReadFromExcelAsync(file);
            }
            var keyList = keys.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
            keyList = keyList.Where(key => !String.IsNullOrEmpty(key)).ToList();
            var command = new AddKeysCommand(id, keyList);
            await _sender.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> ReadFromExcelAsync(IFormFile? file)
        {
            string result = string.Empty;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            if (file != null && file.Length > 0)
            {
                var uploadDirectory = Path.Combine(_environment.WebRootPath, "Uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadDirectory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (!String.IsNullOrEmpty(reader.GetString(i)))
                                    {
                                        result += reader.GetString(i) + "\r\n";
                                    }
                                }
                            }
                        }
                        while (reader.NextResult());
                    }
                }
            }
            return result;
        }
    }
}
